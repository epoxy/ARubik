using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Vuforia;
using System.Collections.Generic;

public class DistanceCalc : MonoBehaviour {
	public UnityEngine.UI.Text instructionsLabel;

	public GameObject ResetGazebutton;
    public GameObject TopLayerGazebutton;
    public GameObject BottomLayerGazebutton;
    public GameObject UnderLayerGazebutton;
    public GameObject LeftTopGazeButton;
    public GameObject RightTopGazeButton;
    public GameObject LeftBottomGazeButton;
    public GameObject RightBottomGazeButton;
    public GameObject LeftUnderGazeButton;
    public GameObject RightUnderGazeButton;

	public GameObject LeftBottomLayerAlgorithm;
	public GameObject RightBottomLayerAlgorithm;

	public ProgressBarScript progressBar;
	public RotateCube rubix;

	private StateManager sm;

	public GameObject wSphereAB;
	public GameObject wSphereBA;
	public GameObject wSphereAC;
	public GameObject wSphereCA;
	public GameObject wSphereCD;
	public GameObject wSphereDC;

	public GameObject ySphereAB;
	public GameObject ySphereBA;
	public GameObject ySphereAC;
	public GameObject ySphereCA;
	public GameObject ySphereCD;
	public GameObject ySphereDC;

	public GameObject TargetArrowAB;
	public GameObject TargetArrowAC;
	public GameObject TargetArrowCD;

	private Vector3 originalPositionAB;
	private Vector3 originalPositionBA;

	double distanceThreshold = 0.6;

	private bool WPlacementTop;
	private bool WPlacementBottom;
	private bool WPlacementUnder;
	private bool WSelectedLeft;
	private bool WSelectedRight;

	private GameModel game;
	private CubeModel cube;
	private InstructionsModel instructions;

    private enum VerticalLocation { TOP, BOTTOM, UNDER };

    // Use this for initialization
    void Start () {
		// Get the Vuforia StateManager
		 sm = TrackerManager.Instance.GetStateManager ();

		//Init text
		instructionsLabel.text = "Find the WHITE piece with RED and GREEN sides.\nHold the cube so it's in the upper left corner.";

		// Init model
		game = new GameModel();
		cube = new CubeModel ();
		instructions = new InstructionsModel ();

		progressBar.reset ();

		//Init varibales for White
		cube.foundA = false;
		cube.foundB = false;
		cube.foundC = false;
		cube.foundD = false;

		cube.matchedAB = false;
		cube.matchedAC = false;
		cube.matchedCD = false;

		instructions.findA = true;
		instructions.findB = false;
		instructions.findC = false;
		instructions.findD = false;

		instructions.matchAB = false;
		instructions.matchAC = false;
		instructions.matchCD = false;

		instructions.animationShowing = false;
	
		resetPlacementButtons ();

		hideWPlacementButtons ();
		hideLeftRightButtons ();

		TargetArrowAB.gameObject.SetActive (false);
		TargetArrowAC.gameObject.SetActive (false);
		TargetArrowCD.gameObject.SetActive (false);

		disableAnimations ();

		game.whiteLayerCompleted = false; 	// Step 1
		game.step2Completed = false;  		// Step 2
		game.yellowLayerCompleted = false;	// Step 3
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.touchCount > 0) {
			RestartGame ();
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)){
			RestartGame ();

		}

		
		iterateTrackables ();

		solveWhiteLayer ();

		// Step #
//		if (!game.whiteLayerCompleted) { // Step 1
//			solveWhiteLayer ();
//		} else if (!game.yellowLayerCompleted) { // Step 2 & 3
//			solveYellowLayer ();
//		} else {
//			instructionsLabel.text = "Winning!";
//		}
	}

	private void iterateTrackables() {
		// Query the StateManager to retrieve the list of  currently 'active' trackables 
		//(i.e. the ones currently being tracked by Vuforia)
		IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();

		// Iterate through the list of active trackables
		//Debug.Log ("List of trackables currently active (tracked): ");
		foreach (TrackableBehaviour tb in activeTrackables) {

			if (!game.whiteLayerCompleted) {
				if(string.Equals(tb.TrackableName,"FrameMarker0")){
					cube.foundA = true;
				}
				if(cube.foundA && string.Equals(tb.TrackableName,"FrameMarker1")){
					cube.foundB = true;
				}
				if(cube.matchedAB && string.Equals(tb.TrackableName,"FrameMarker2")){
					cube.foundC = true;
				}
				if(cube.matchedAC && string.Equals(tb.TrackableName,"FrameMarker3")){
					cube.foundD = true;
				}
			}
				
		}
	}

	private void solveWhiteLayer(){

		// Calculate distances
		float distanceAB = Vector3.Distance (wSphereAB.transform.position, wSphereBA.transform.position);
		float distanceAC = Vector3.Distance (wSphereAC.transform.position, wSphereCA.transform.position);
		float distanceCD = Vector3.Distance (wSphereCD.transform.position, wSphereDC.transform.position);

		//Instructions
		if (cube.foundA && instructions.findA) {
			updateProgressBar (1);
			instructionsLabel.text = "Find the WHITE piece with GREEN and ORANGE sides somewhere on the cube.";
			instructions.findA = false;
			instructions.findB = true;
		}
			
		if (cube.foundB && instructions.findB) { //Match A & B
			instructionsLabel.text = "Where is this piece located?\n Look back at the first piece and select the location.";
			instructions.findB = false;
			instructions.matchAB = true;
			showWPlacementButtons ();
		}

		if (cube.foundC && instructions.findC) { // Match A & C
			instructionsLabel.text = "Where is this piece located?\n Look back at the first piece and select the location.";
			instructions.findC = false;
			instructions.matchAC = true;
			showWPlacementButtons ();
		}

		if (cube.foundD && instructions.findD) { //Match C & D
			instructionsLabel.text = "Where is this piece located?\n Look back at the first piece and select the location.";
			instructions.findD = false;
			instructions.matchCD = true;
			showWPlacementButtons ();
		}

		if (instructions.matchAB && !cube.matchedAB) {
			matchWhiteLayerInstructions ();
		}
		if (instructions.matchAC && !cube.matchedAC) {
			matchWhiteLayerInstructions ();
		}
		if (instructions.matchCD && !cube.matchedCD) {
			matchWhiteLayerInstructions ();
		}

		// Distance 
		if (instructions.matchAB && distanceThreshold > distanceAB) {
			updateProgressBar(3);
			updateProgressBar(4);
			updateProgressBar(5);
			instructionsLabel.text = "Find the WHITE piece with RED and BLUE sides somewhere on the cube";
			cube.matchedAB = true;
			instructions.matchAB = false;
			instructions.findC = true;
			hideWPlacementButtons ();
			hideLeftRightButtons ();
			TargetArrowAB.gameObject.SetActive (false);
			disableAnimations ();
			resetPlacementButtons ();
		}
		if (instructions.matchAC && distanceThreshold > distanceAC) {
			updateProgressBar(6);
			updateProgressBar(7);
			updateProgressBar(8);
			updateProgressBar(9);
			instructionsLabel.text = "Find the WHITE piece with BLUE and ORANGE sides somewhere on the cube";
			cube.matchedAC = true;
			instructions.matchAC = false;
			instructions.findD = true;
			hideWPlacementButtons ();
			TargetArrowAC.gameObject.SetActive (false);
			resetPlacementButtons ();
		}
		if (instructions.matchCD && distanceThreshold > distanceCD) {
			updateProgressBar(10);
			updateProgressBar(11);
			updateProgressBar(12);
			updateProgressBar(13);
			instructionsLabel.text = "White layer completed.\nSolve the yellow layer";
			cube.matchedCD = true;
			instructions.matchCD = false;
			game.whiteLayerCompleted = true;
			instructions.findYA = true;
			hideWPlacementButtons ();
			TargetArrowCD.gameObject.SetActive (false);
			resetPlacementButtons ();
		}
		//Debug.Log (distanceAB);
	}

	private void matchWhiteLayerInstructions() {
		if (!instructions.animationShowing) {
			if (WSelectedLeft) {
				instructionsLabel.text = "";
				instructions.animationShowing = true;
				LeftBottomLayerAlgorithm.gameObject.SetActive (true);
				hideLeftRightButtons ();
			} else if (WSelectedRight) {
				instructionsLabel.text = "";
				RightBottomLayerAlgorithm.gameObject.SetActive (true);
				instructions.animationShowing = true;
				hideLeftRightButtons ();
			} else if (WPlacementBottom) {
				if (instructions.matchAB)
					TargetArrowAB.gameObject.SetActive (true);
				if (instructions.matchAC)
					TargetArrowAC.gameObject.SetActive (true);
				if (instructions.matchCD)
					TargetArrowCD.gameObject.SetActive (true);
				instructionsLabel.text = "Rotate the under layer so that the white piece is underneath its target.\nSelect which side it's on.";
				hideWPlacementButtons ();
				showLeftRightButtons (VerticalLocation.BOTTOM);
			} else if (WPlacementTop) { // Select where it is located
				updateProgressBar (2);
				instructionsLabel.text = "";
				//TODO Add next step
			} else if (WPlacementUnder) {
				updateProgressBar (2);
				instructionsLabel.text = "";
				//TODO Add next step
			} 
		}

	}

	private void solveYellowLayer(){

		// Calculate distances
		float distanceAB = Vector3.Distance (ySphereAB.transform.position, ySphereBA.transform.position);
		float distanceAC = Vector3.Distance (ySphereAC.transform.position, ySphereCA.transform.position);
		float distanceCD = Vector3.Distance (ySphereCD.transform.position, ySphereDC.transform.position);

		Debug.Log (distanceAB);
		Debug.Log (distanceAC);
		Debug.Log (distanceCD);

		// Yellow layers are in right position 
		if (distanceThreshold > distanceAB && distanceThreshold > distanceAC && distanceThreshold > distanceCD) {
			game.yellowLayerCompleted = true;
		}

		// Case #
		//Instructions
//		if (yCase1) {
//			instructions.text = "Case 1: R' U' R U' R' U2 R";
//		}
//		if (yCase2) {
//			instructions.text = "Case 2: L U L' U L U2 L'";
//		}
//		if (yCase3) {
//			instructions.text = "Case 3: R2 U2 R U2 R2";
//		}
//		if (yCase4) {
//			instructions.text = "Case 4: F [R U R' U'] [R U R' U'] F''";
//		}
//		if (yCase5) {
//			instructions.text = "Case 5: F [R U R' U'] F'";
//		}
//		if (yCase6) {
//			instructions.text = "Case 6: [R U R' U'] [R' F R F']";
//		}
//		if (yCase7) {
//			instructions.text = "Case 7: [F R U' R' U' R U R' F']";
//		}
//
//		if (step2Completed) {
//			instructions.text = "Step 3 (and last): L' U R' D2 R U' R' D2 R2";
//		}
	}

	public void RestartGame () {
        Debug.Log ("Reset");
		Start();
	}

    //TODO: For selectedPlacement, create Enum or something else that is more readable than int-values
    public void SelectWPlacement (int selectedPlacement) {
		if (selectedPlacement == 1) {
			WPlacementTop = true;
		}
		if (selectedPlacement == 2) {
			WPlacementBottom = true;
		}
		if (selectedPlacement == 3) {
			WPlacementUnder = true;
		}
		//disableYCaseButtons ();
	}

    //TODO: For selectedLeftRight, create Enum or something else that is more readable than int-values
    public void SelectLeftRight (int selectedLeftRight) {
		if (selectedLeftRight == 1) {
			WSelectedLeft = true;
		}
		if (selectedLeftRight == 2) {
			WSelectedRight = true;
		}
		//disableYCaseButtons ();
	}
		
	public void SelectYCase (int selectedCase) {
//		if (selectedCase == 1) {
//			yCase1 = true;
//		}
//		if (selectedCase == 2) {
//			yCase2 = true;
//		}
//		hideYCaseButtons ();
	}

	private void showWPlacementButtons(){
        //TODO Hide buttons that are not needed
        //Gaze buttons
        TopLayerGazebutton.gameObject.SetActive(true);
        BottomLayerGazebutton.gameObject.SetActive(true);
        UnderLayerGazebutton.gameObject.SetActive(true);
    }

	private void hideWPlacementButtons(){
        //Gaze buttons
        TopLayerGazebutton.gameObject.SetActive(false);
        BottomLayerGazebutton.gameObject.SetActive(false);
        UnderLayerGazebutton.gameObject.SetActive(false);
	}

	private void showLeftRightButtons(VerticalLocation verticalLocation){
        //Show correct buttons
        //TODO: Put all buttons to x-axis where z=0. Keep them separeted for now until tested properly. //Anton
        switch (verticalLocation)
        {
            case VerticalLocation.TOP:
                Debug.Log("TOP");
                //Show left right buttons for top
                LeftTopGazeButton.gameObject.SetActive(true);
                RightTopGazeButton.gameObject.SetActive(true);
                break;
            case VerticalLocation.BOTTOM:
                Debug.Log("BOTTOM");
                //Show left right buttons for bottom
                LeftBottomGazeButton.gameObject.SetActive(true);
                RightBottomGazeButton.gameObject.SetActive(true);
                break;
            case VerticalLocation.UNDER:
                Debug.Log("UNDER");
                //Show left right buttons for under
                LeftUnderGazeButton.gameObject.SetActive(true);
                RightUnderGazeButton.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

	private void hideLeftRightButtons(){
        //Gaze buttons
        LeftTopGazeButton.gameObject.SetActive(false);
        RightTopGazeButton.gameObject.SetActive(false);
        LeftBottomGazeButton.gameObject.SetActive(false);
        RightBottomGazeButton.gameObject.SetActive(false);
        LeftUnderGazeButton.gameObject.SetActive(false);
        RightUnderGazeButton.gameObject.SetActive(false);
    }

	private void disableAnimations() {
		LeftBottomLayerAlgorithm.gameObject.SetActive(false);
		RightBottomLayerAlgorithm.gameObject.SetActive (false);
	}

	private void updateProgressBar(int setLevel){
		progressBar.level = setLevel;
		progressBar.upLevel ();
	}

	private void resetPlacementButtons(){
		// Init buttons 
		WPlacementTop = false;
		WPlacementBottom = false;
		WPlacementUnder = false;
		WSelectedLeft = false;
		WSelectedRight = false;
	}
		
}

class GameModel
{
	public bool whiteLayerCompleted;
	public bool step2Completed;
	public bool yellowLayerCompleted;
}

class InstructionsModel
{
	public bool findA;
	public bool findB;
	public bool findC;
	public bool findD;

	public bool matchAB;
	public bool matchAC;
	public bool matchCD;

	public bool findYA;

	public bool animationShowing;

	public bool yCase1;
	public bool yCase2;
	public bool yCase3;
	public bool yCase4;
	public bool yCase5;
	public bool yCase6;
	public bool yCase7;
}

class CubeModel
{
	public bool foundA;
	public bool foundB;
	public bool foundC;
	public bool foundD;

	public bool matchedAB;
	public bool matchedAC;
	public bool matchedCD;

	public bool foundYA;
	public bool foundYB;
	public bool foundYC;
	public bool foundYD;
}
	