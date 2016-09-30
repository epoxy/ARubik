using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DistanceCalc : MonoBehaviour {
	public GameObject sphere0;
	public GameObject sphere1;
	public UnityEngine.UI.Text instructions;
	public UnityEngine.UI.Button reset;
	//public GameObject GC;

	//public GameObject atomicbomb;
	// Use this for initialization
	void Start () {
		//GC.SetActive(false);
		//instructions = GetComponent<UnityEngine.UI.Text>();
		instructions.text = "Find A";
		Vector3 originalPosition0 = sphere0.transform.position;
		Vector3 originalPosition1 = sphere1.transform.position;
		float originalDistance = Vector3.Distance (sphere0.transform.position, sphere1.transform.position);
		Debug.Log (originalPosition0);
		Debug.Log (originalPosition1);
		Debug.Log (originalDistance);
	
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (sphere0.transform.position, sphere1.transform.position);
		double threshold = 0.8;

	
		if (distance > threshold) {
		//	GC.SetActive(false);
		//	Debug.Log ("Unmatch");
		//  instructions.text = "Find A";
		}
		if (threshold > distance) {
			//Debug.Log ("Match");
			//GC.SetActive(true);
			instructions.text = "Find C";
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
