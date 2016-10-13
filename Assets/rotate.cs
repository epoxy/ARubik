using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

	public Camera cam;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = cam.transform.rotation;
	}
}
