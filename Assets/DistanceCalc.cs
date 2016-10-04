using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Vuforia;
using System.Collections.Generic;

public class DistanceCalc : MonoBehaviour {
	public GameObject sphere0;
	public GameObject sphere1;
	public UnityEngine.UI.Text instructions;
	public UnityEngine.UI.Button reset;

	private bool foundA;
	private bool foundB;

	private bool findA;
	private bool findB;

	private bool matchedAB;

	private bool whiteLayerCompleted;

	// Use this for initialization
	void Start () {
		//Init text
		instructions.text = "Find A";
		Vector3 originalPosition0 = sphere0.transform.position;
		Vector3 originalPosition1 = sphere1.transform.position;
		float originalDistance = Vector3.Distance (sphere0.transform.position, sphere1.transform.position);
		Debug.Log (originalPosition0);
		Debug.Log (originalPosition1);
		Debug.Log (originalDistance);

		//Init varibales
		foundA = false;
		foundB = false;

		findA = true;
		findB = false;

		matchedAB = false;

		whiteLayerCompleted = false;

	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (sphere0.transform.position, sphere1.transform.position);
		double threshold = 0.8;

		// Get the Vuforia StateManager
		StateManager sm = TrackerManager.Instance.GetStateManager ();

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
		}

		if (foundA && findA) {
			instructions.text = "Find B";
			findA = false;
			findB = true;
		}

		if (foundB && findB) {
			instructions.text = "Match A & B";
		}

	
		if (distance > threshold) {
		//	GC.SetActive(false);
		//	Debug.Log ("Unmatch");
		//  instructions.text = "Find A";
		}
		if (threshold > distance) {
			//Debug.Log ("Match");
			//GC.SetActive(true);
			instructions.text = "Find C";
			matchedAB = true;
			findB = false;
			//GC.transform.position = Vector3.MoveTowards(GC.transform.position, new Vector3(-80,600,0), 20*Time.deltaTime); // Vector3.zero
		} 
		//Debug.Log (distance);
	}

	public void RestartGame ()
	{
		Debug.Log ("Reset");
		Start();
	}
}
