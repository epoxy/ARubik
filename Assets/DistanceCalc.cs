using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class DistanceCalc : MonoBehaviour {
	public UnityEngine.UI.Text instructions;
	public UnityEngine.UI.Button reset;

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

	private bool whiteLayerCompleted;
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

		whiteLayerCompleted = false;
		yellowLayerCompleted = false;

		//resetGameObjectPositions ();

//		float originalDistance = Vector3.Distance (sphere0.transform.position, sphere1.transform.position);
//		Debug.Log (originalPosition0);
//		Debug.Log (originalPosition1);
//		Debug.Log (originalDistance);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Query the StateManager to retrieve the list of
		// currently 'active' trackables 
		//(i.e. the ones currently being tracked by Vuforia)
		IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();

		// Iterate through the list of active trackables
		//Debug.Log ("List of trackables currently active (tracked): ");
		foreach (TrackableBehaviour tb in activeTrackables) {
			//Debug.Log("Trackable: " + tb.TrackableName);
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
			if(matchedCD && string.Equals(tb.TrackableName,"FrameMarker4")){
				foundYA = true;
			}
		}

		if (!whiteLayerCompleted) {
			solveWhiteLayer ();
		} else if (!yellowLayerCompleted) {
			solveYellowLayer ();
		} else {
			//Winning mode!
			instructions.text = "Winning!";
		}


	}

	private void solveWhiteLayer(){

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
		}
		//Debug.Log (distanceAB);
	}

	private void solveYellowLayer(){
		float distanceAB = Vector3.Distance (ySphereAB.transform.position, ySphereBA.transform.position);
		float distanceAC = Vector3.Distance (ySphereAC.transform.position, ySphereCA.transform.position);
		float distanceCD = Vector3.Distance (ySphereCD.transform.position, ySphereDC.transform.position);

		Debug.Log (distanceAB);
		Debug.Log (distanceAC);
		Debug.Log (distanceCD);
		// Dinstace 
		if (distanceThreshold > distanceAB && distanceThreshold > distanceAC && distanceThreshold > distanceCD) {
			yellowLayerCompleted = true;
		}
	}

	public void RestartGame ()
	{
		Debug.Log ("Reset");
		Start();
	}

	private void resetGameObjectPositions(){

//		sphereAB.transform.position = new Vector3(1.9f, 0.0f, 2.5f);
//		sphereBA.transform.position = new Vector3(3.1f, 0.0f, 2.5f);
//		sphereAC.transform.position = new Vector3(1.5f, 0.0f, 2.1f);
//		sphereCA.transform.position = new Vector3(1.5f, 0.0f, 0.9f);
//		sphereCD.transform.position = new Vector3(1.9f, 0.0f, 0.5f);
//		sphereDC.transform.position = new Vector3(3.1f, 0.0f, 0.5f);
	
	}
}
