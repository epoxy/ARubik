﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class DistanceCalc : MonoBehaviour {
	public UnityEngine.UI.Text instructionsLabel;
    
	public GameObject ResetGazebutton;
    public GameObject TopLayerGazebutton;
    public GameObject BottomLayerGazebutton;
    public GameObject UnderLayerGazebutton;
    public GameObject LeftGazebutton;
    public GameObject RightGazebutton;

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
	public GameObject GlowB;

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

	// Use this for initialization
	void Start () {
		// Get the Vuforia StateManager
		 sm = TrackerManager.Instance.GetStateManager ();

		//Init text
		instructionsLabel.text = "Find A and put it in upper left corner";

		// Init model
		game = new GameModel();
		cube = new CubeModel ();
		instructions = new InstructionsModel ();

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

	
		// Init buttons 
		WPlacementTop = false;
		WPlacementBottom = false;
		WPlacementUnder = false;
		WSelectedLeft = false;
		WSelectedRight = false;

		hideWPlacementButtons ();
		hideLeftRightButtons ();

		TargetArrowAB.gameObject.SetActive (false);
		GlowB.gameObject.SetActive (false);

		game.whiteLayerCompleted = false; 	// Step 1
		game.step2Completed = false;  		// Step 2
		game.yellowLayerCompleted = false;	// Step 3
	}
	
	// Update is called once per frame
	void Update () {
		
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
			Debug.Log("Trackable: " + tb.Trackable.ID);

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
			instructionsLabel.text = "Find B";
			instructions.findA = false;
			instructions.findB = true;
			TargetArrowAB.gameObject.SetActive (true);
			GlowB.gameObject.SetActive (true);
		}
			
		if (cube.foundB && instructions.findB) { //Match A & B
			instructionsLabel.text = "Look at A. Where is B located?";
			instructions.findB = false;
			instructions.matchAB = true;
			showWPlacementButtons ();
		}

		if (cube.foundC && instructions.findC) {
			instructionsLabel.text = "Match A & C";
			instructions.findC = false;
			instructions.matchAC = true;
			showWPlacementButtons ();
		}

		if (cube.foundD && instructions.findD) {
			instructionsLabel.text = "Match C & D";
			instructions.findD = false;
			instructions.matchCD = true;
			showWPlacementButtons ();
		}

		if (instructions.matchAB && !cube.matchedAB) {
			matchABInstructions ();
		}

		// Dinstace 
		if (instructions.matchAB && distanceThreshold > distanceAB) {
			instructionsLabel.text = "Find C";
			cube.matchedAB = true;
			instructions.matchAB = false;
			instructions.findC = true;
			hideWPlacementButtons ();
			hideLeftRightButtons ();
			TargetArrowAB.gameObject.SetActive (false);
		}
		if (instructions.matchAC && distanceThreshold > distanceAC) {
			instructionsLabel.text = "Find D";
			cube.matchedAC = true;
			instructions.matchAC = false;
			instructions.findD = true;
			hideWPlacementButtons ();
		}
		if (instructions.matchCD && distanceThreshold > distanceCD) {
			instructionsLabel.text = "White layer completed!";
			cube.matchedCD = true;
			instructions.matchCD = false;
			game.whiteLayerCompleted = true;
			instructions.findYA = true;
			hideWPlacementButtons ();
		}
		//Debug.Log (distanceAB);
	}

	private void matchABInstructions() {
		// Select where it is located
		if (WPlacementTop) {
			instructionsLabel.text = "It is at the top. Put it at the bottom layer";
		}
		if (WPlacementBottom) {
			instructionsLabel.text = "Put B under the target. Then,  looking at B. Is it to the left or right? ";
			hideWPlacementButtons ();
			showLeftRightButtons ();
		}
		if (WPlacementUnder) {
			instructionsLabel.text = "It is under. Put it on the bottom layer";
		}

		if (WSelectedLeft) {
			instructionsLabel.text = "Left algorithm";
		}
		if (WSelectedRight) {
			instructionsLabel.text = "Right algorithm";
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

	private void showLeftRightButtons(){
        //Gaze buttons
		LeftGazebutton.gameObject.SetActive(true);
		RightGazebutton.gameObject.SetActive(true);
    }

	private void hideLeftRightButtons(){
        //Gaze buttons
		LeftGazebutton.gameObject.SetActive(false);
		RightGazebutton.gameObject.SetActive(false);
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