using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class DistanceCalc : MonoBehaviour {
	public UnityEngine.UI.Text instructions;
	public UnityEngine.UI.Button reset;

	public GameObject sphereAB;
	public GameObject sphereBA;
	public GameObject sphereAC;
	public GameObject sphereCA;
	public GameObject sphereCD;
	public GameObject sphereDC;

	private StateManager sm;

	double distanceThreshold = 0.8;

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

	private bool whiteLayerCompleted;

	// Use this for initialization
	void Start () {
		// Get the Vuforia StateManager
		 sm = TrackerManager.Instance.GetStateManager ();

		//Init text
		instructions.text = "Find A";

		//Init varibales
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


		whiteLayerCompleted = false;


//		Vector3 originalPosition0 = sphere0.transform.position;
//		Vector3 originalPosition1 = sphere1.transform.position;
//		float originalDistance = Vector3.Distance (sphere0.transform.position, sphere1.transform.position);
//		Debug.Log (originalPosition0);
//		Debug.Log (originalPosition1);
//		Debug.Log (originalDistance);
	}
	
	// Update is called once per frame
	void Update () {
		float distanceAB = Vector3.Distance (sphereAB.transform.position, sphereBA.transform.position);
		float distanceAC = Vector3.Distance (sphereAC.transform.position, sphereCA.transform.position);
		float distanceCD = Vector3.Distance (sphereCD.transform.position, sphereDC.transform.position);

		// Query the StateManager to retrieve the list of
		// currently 'active' trackables 
		//(i.e. the ones currently being tracked by Vuforia)
		IEnumerable<TrackableBehaviour> activeTrackables = sm.GetActiveTrackableBehaviours ();

		// Iterate through the list of active trackables
		//Debug.Log ("List of trackables currently active (tracked): ");
		foreach (TrackableBehaviour tb in activeTrackables) {
			Debug.Log("Trackable: " + tb.TrackableName);
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
			instructions.text = "DONE!";
			matchedCD = true;
			matchCD = false;
			whiteLayerCompleted = true;
		}

	}

	public void RestartGame ()
	{
		Debug.Log ("Reset");
		Start();
	}
}
