using UnityEngine;
using System.Collections;

public class DistanceCalc : MonoBehaviour {
	public GameObject sphere0;
	public GameObject sphere1;
	public GameObject GC;

	//public GameObject atomicbomb;
	// Use this for initialization
	void Start () {
		GC.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (sphere0.transform.position, sphere1.transform.position);
		double threshold = 0.5;

		if (distance > threshold) {
			GC.SetActive(false);
		//	Debug.Log ("Unmatch");
		}
		if (threshold > distance) {
		//	Debug.Log ("Match");
			GC.SetActive(true);
			//GC.transform.position = Vector3.MoveTowards(GC.transform.position, new Vector3(-80,600,0), 20*Time.deltaTime); // Vector3.zero
		} 
		//Debug.Log (distance);
	}
}
