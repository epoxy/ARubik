using UnityEngine;
using System.Collections;

public class LoopGif : MonoBehaviour {
    private Renderer renderer;
    public Texture[] frames = null;
    private int framesPerSecond = 4;

    // Use this for initialization
    void Start () {
        Debug.Log("Gif Start");
        renderer = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        //Iterate over the frames
        int index = (int)(Time.time * framesPerSecond) % frames.Length;
        renderer.material.mainTexture = frames[index];

    }
}