using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class DistanceCalc : MonoBehaviour {
	public UnityEngine.UI.Text instructions;
	public UnityEngine.UI.Button reset;
	public UnityEngine.UI.Button YCase1Button;
	public UnityEngine.UI.Button YCase2Button;

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

	private Vector3 originalPositionAB;
	private Vector3 originalPositionBA;

	double distanceThreshold = 0.6;

	private bool foundA;
	private bool foundB;
	private bool foundC;
	private bool foundD;

	private bool matchedAB;
	private bool matchedAC;
	private bool matchedCD;

	private bool findA;
	private bool findB;
	private bool findC;
	private bool findD;

	private bool matchAB;
	private bool matchAC;
	private bool matchCD;

	private bool foundYA;
	private bool foundYB;
	private bool foundYC;
	private bool foundYD;

	private bool findYA;

	private bool yCase1;
	private bool yCase2;
	private bool yCase3;
	private bool yCase4;
	private bool yCase5;
	private bool yCase6;
	private bool yCase7;

	private bool whiteLayerCompleted;
	private bool step2Completed;
	private bool yellowLayerCompleted;

	// Use this for initialization
	void Start () {
		// Get the Vuforia StateManager
		 sm = TrackerManager.Instance.GetStateManager ();

		//Init text
		instructions.text = "Find A";

		//Init varibales
		//White
		foundA = false;
		foundB = false;
		foundC = false;
		foundD = false;

		matchedAB = false;
		matchedAC = false;
		matchedCD = false;

		findA = true;
		findB = false;
		findC = false;
		findD = false;

		matchAB = false;
		matchAC = false;
		matchCD = false;

		//Yellow
		foundYA = false;
		foundYB = false;
		foundYC = false;
		foundYD = false;

		findYA = false;

		yCase1 = false;
		yCase2 = false;
		yCase3 = false;
		yCase4 = false;
		yCase5 = false;
		yCase6 = false;
		yCase7 = false;

		YCase1Button.gameObject.SetActive(false);
		YCase2Button.gameObject.SetActive(false);

		whiteLayerCompleted = false; 	// Step 1
		step2Completed = false;  		// Step 2
		yellowLayerCompleted = false;	// Step 3
	}
	
	// Update is called once per frame
	void Update () {
		
		iterateTrackables ();

		// Step #
		if (!whiteLayerCompleted) { // Step 1
			solveWhiteLayer ();
		} else if (!yellowLayerCompleted) { // Step 2 & 3
			solveYellowLayer ();
		} else {
			instructions.text = "Winning!";
		}
	}

	private void iterateTrackables() {
		// Query the StateManager to retrieve the list of  currently 'active' trackables 
		//(i.e. the ones currently being tracked by Vuforia)
		IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();

		// Iterate through the list of active trackables
		//Debug.Log ("List of trackables currently active (tracked): ");
		foreach (TrackableBehaviour tb in activeTrackables) {
			//Debug.Log("Trackable: " + tb.TrackableName);

			if (!whiteLayerCompleted) {
				if(string.Equals(tb.TrackableName,"FrameMarker0")){
					foundA = true;
				}
				if(foundA && string.Equals(tb.TrackableName,"FrameMarker1")){
					foundB = true;
				}
				if(matchedAB && string.Equals(tb.TrackableName,"FrameMarker2")){
					foundC = true;
				}
				if(matchedAC && string.Equals(tb.TrackableName,"FrameMarker3")){
					foundD = true;
				}
			}

			if (whiteLayerCompleted) {
//				if(string.Equals(tb.TrackableName,"FrameMarker4")){
//					YCase1 = true;
//				}
//				if(string.Equals(tb.TrackableName,"FrameMarker5")){
//					YCase2 = true;
//				}
//				if(string.Equals(tb.TrackableName,"FrameMarker6")){
//					YCase3 = true;
//				}
//				if(string.Equals(tb.TrackableName,"FrameMarker7")){
//					step2Completed = true;
//				}
			}
		}
	}

	private void solveWhiteLayer(){

		// Calculate distances
		float distanceAB = Vector3.Distance (wSphereAB.transform.position, wSphereBA.transform.position);
		float distanceAC = Vector3.Distance (wSphereAC.transform.position, wSphereCA.transform.position);
		float distanceCD = Vector3.Distance (wSphereCD.transform.position, wSphereDC.transform.position);

		//Instructions
		if (foundA && findA) {
			instructions.text = "Find B";
			findA = false;
			findB = true;
		}

		if (foundB && findB) {
			instructions.text = "Match A & B";
			findB = false;
			matchAB = true;
		}

		if (foundC && findC) {
			instructions.text = "Match A & C";
			findC = false;
			matchAC = true;
		}

		if (foundD && findD) {
			instructions.text = "Match C & D";
			findD = false;
			matchCD = true;
		}

		// Dinstace 
		if (matchAB && distanceThreshold > distanceAB) {
			instructions.text = "Find C";
			matchedAB = true;
			matchAB = false;
			findC = true;
		}
		if (matchAC && distanceThreshold > distanceAC) {
			instructions.text = "Find D";
			matchedAC = true;
			matchAC = false;
			findD = true;
		}
		if (matchCD && distanceThreshold > distanceCD) {
			instructions.text = "White layer completed!";
			matchedCD = true;
			matchCD = false;
			whiteLayerCompleted = true;
			findYA = true;
			enableYCaseButtons ();
		}
		//Debug.Log (distanceAB);
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
			yellowLayerCompleted = true;
		}

		// Case #
		//Instructions
		if (yCase1) {
			instructions.text = "Case 1: R' U' R U' R' U2 R";
		}
		if (yCase2) {
			instructions.text = "Case 2: L U L' U L U2 L'";
		}
		if (yCase3) {
			instructions.text = "Case 3: R2 U2 R U2 R2";
		}
		if (yCase4) {
			instructions.text = "Case 4: F [R U R' U'] [R U R' U'] F''";
		}
		if (yCase5) {
			instructions.text = "Case 5: F [R U R' U'] F'";
		}
		if (yCase6) {
			instructions.text = "Case 6: [R U R' U'] [R' F R F']";
		}
		if (yCase7) {
			instructions.text = "Case 7: [F R U' R' U' R U R' F']";
		}

		if (step2Completed) {
			instructions.text = "Step 3 (and last): L' U R' D2 R U' R' D2 R2";
		}
	}

	public void RestartGame () {
		Debug.Log ("Reset");
		Start();
	}
		
	public void SelectYCase (int selectedCase) {
		if (selectedCase == 1) {
			yCase1 = true;
		}
		if (selectedCase == 2) {
			yCase2 = true;
		}
		disableYCaseButtons ();
	}

	private void enableYCaseButtons(){
		YCase1Button.gameObject.SetActive(true);
		YCase2Button.gameObject.SetActive(true);
	}

	private void disableYCaseButtons(){
		YCase1Button.gameObject.SetActive(false);
		YCase2Button.gameObject.SetActive(false);
	}
}
