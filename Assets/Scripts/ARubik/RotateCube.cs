using UnityEngine;
using System.Collections;
using System;

public class RotateCube : MonoBehaviour {

	public GameObject rubix;
	public int speed = 10;

	private GameObject[] cubes;
    private GameObject[] side;
    private GameObject pivot;
    private int i;

   // private string[] start = new string[4]{"rprime", "dprime", "r", "d"};
    //private string[] scramble = new string[4] {"d", "l", "dprime", "lprime"};
	private string[] scramble = new string[6] {"l", "f", "u","uprime", "fprime", "lprime"};


    private int scrambleIndex = 0; // Keeps track of where in the scramble array we are 


    private float totalRotation = 0; // Keeps track of how much a side has already rotated
 	private float rotationAmount = 90f; // We only want to rotate 90 degrees at a time

 	private string rot; // string to indicate what rotation is on, i.e. r, rprime, l, lprime
	void Start () {

		//All cubes tagged as Cube
         cubes = GameObject.FindGameObjectsWithTag("Cube");

         //4 cubes per side
         side = new GameObject[4];

         pivot = new GameObject("Pivot");

         
         //Set up everything for the first rotation, which side should be rotated.
         nextRotation();

	}

//	public void setScramble(string algorithm){
//		private string[] scramble = new string[6] {"l", "f", "u","uprime", "fprime", "lprime"};
//	}

	// Update is called once per frame
	void Update () {

		/*rubix.transform.RotateAround(transform.position, transform.up, Time.deltaTime * 30f);
		if(!up){
			rubix.transform.RotateAround(transform.position, transform.right, Time.deltaTime * -10f);
			b += Time.deltaTime*-10f;
			print("B: " + b);
			if(b <= -45){
				up = true;

			}
		}
		if(up){
			rubix.transform.RotateAround(transform.position, transform.right, Time.deltaTime * 10f);
			b += Time.deltaTime*10f;
			print("B: " + b);
			if(b >=  45){
				up = false;
			}
		}*/

		//printCubePositions ();

		switch(rot){
			case "r":
				R();
				break;
			case "rprime":
				Rprime();
				break;
			case "l":
				L();
				break;
			case "lprime":
				Lprime();
				break;
			case "u":
				U();
				break;
			case "uprime":
				Uprime();
				break;
			case "d":
				D();
				break;
			case "dprime":
				Dprime();
				break;
			case "f":
				F();
				break;
			case "fprime":
				Fprime();
				break;
			case "b":
				B();
				break;
			case "bprime":
				Bprime();
				break;
		}
 		
 			


    }

    void nextRotation(){

    	roundCubePositions();
    	//printCubePositions();

    	if(scrambleIndex < scramble.Length){
	    	rot = scramble[scrambleIndex];
	    	scrambleIndex++;
	    	totalRotation = 0;
	    	removePivot(); 	//Remove used pivot
	    	SetPivot();		//Set a new pivot
    	}	
    }

    void removePivot(){
    	pivot.transform.DetachChildren();
    	foreach(GameObject cube in cubes)
         {
	        cube.transform.parent = rubix.transform;
	         
	    }
	    Destroy(pivot);
    }

    //Used for debugging
    void printCubePositions(){
    	int j=1;
    	foreach(GameObject cube in cubes){
    		Debug.Log(j + "(" + cube.transform.position.x + ", " + cube.transform.position.y + ", " + cube.transform.position.z +")");
    		j++;
    	}
    }

    void SetPivot(){
	 	//Reset Counter
         i = 0;

         //Create new pivot point
         pivot = new GameObject("Pivot");
         //This point is not world space relative, you would have to add the world space vector of the entire cube first.
         pivot.transform.position = new Vector3(1,-0.5f,3);
         pivot.transform.parent = rubix.transform;
      	
      	 roundCubePositions();
      	
      	//The string rot indicates what rotation is happening.
         switch(rot){
			case "r":
				RightPivot();
				break;
			case "rprime":
				RightPivot();
				break;
			case "l":
				LeftPivot();
				break;
			case "lprime":
				LeftPivot();
				break;
			case "u":
				UpPivot();
				break;
			case "uprime":
				UpPivot();
				break;
			case "d":
				DownPivot();
				break;
			case "dprime":
				DownPivot();
				break;
			case "f":
				FrontPivot();
				break;
			case "fprime":
				FrontPivot();
				break;
			case "b":
				BackPivot();
				break;
			case "bprime":
				BackPivot();
				break;
		}
         
	 }
       
    void roundCubePositions(){
    	foreach(GameObject cube in cubes){
    		float posX = (float) Math.Round(cube.transform.position.x,1);
    		float posY = (float) Math.Round(cube.transform.position.y,1);
    		float posZ = (float) Math.Round(cube.transform.position.z,1);
    		
    		cube.transform.position = new Vector3(posX, posY, posZ);
//    		Debug.Log("rounding up cube pos: "+ cube.transform.position);
//    		Debug.Log("cube rotations: "+ cube.transform.rotation);
    		//cube.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    		
    	}
    }


// THE FOLLOWING FUNCTIONS DO THE ACTUAL ROTATION OF THE CUBES, THEY ARE CALLED FROM THE UPDATE FUNCTION
 	void R(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.x;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * speed), Vector3.right);
             totalRotation += Time.deltaTime * speed;
         }
         else{
         	roundCubePositions();
         	nextRotation();
         }
	}

	void Rprime(){
		if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.x;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle - (Time.deltaTime * speed), Vector3.right);
             totalRotation += Time.deltaTime * speed;
         }
         else{
         	roundCubePositions();
         	nextRotation();
         }
	}

	void L(){
		if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.x;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle - (Time.deltaTime * speed), Vector3.right);
            totalRotation += Time.deltaTime * speed;
        }
         	
        else{
        	roundCubePositions();
         	nextRotation();
        }
	}

	void Lprime(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.x;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * speed), Vector3.right);
            totalRotation += Time.deltaTime * speed;
        }
         	
        else{
        	roundCubePositions();
         	nextRotation();
        }

    }

    void U(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.y;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * speed), Vector3.up);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }

    void Uprime(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.y;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle - (Time.deltaTime * speed), Vector3.up);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }

    void D(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.y;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle - (Time.deltaTime * speed), Vector3.up);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }

    void Dprime(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.y;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * speed), Vector3.up);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }

    void F(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.z;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle - (Time.deltaTime * speed), Vector3.forward);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }

    void Fprime(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.z;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * speed), Vector3.forward);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }

    void B(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.z;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle - (Time.deltaTime * speed), Vector3.forward);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }

    void Bprime(){
    	if(Mathf.Abs (totalRotation) < Mathf.Abs (rotationAmount)){
            float currentAngle = pivot.transform.rotation.eulerAngles.z;
            pivot.transform.rotation = Quaternion.AngleAxis(currentAngle + (Time.deltaTime * speed), Vector3.forward);
            totalRotation += Time.deltaTime * speed;
        }	
        else{
        	roundCubePositions();
         	nextRotation();
        }
    }


// THE FOLLOWING ARE FUNCTIONS THAT FIND WHICH CUBES SHOULD BE TURNED AND PUTS THE PIVOT GAMEOBJECT AS THEIR PARENT
     void RightPivot()
     {
      
   		foreach(GameObject cube in cubes)
         {
			//print("x: " + cube.transform.position.x); 
	        if(cube.transform.position.x == 1.1f)
	        {
	            cube.transform.parent = pivot.transform;
	            side[i] = cube;
	            i++;
	        }
	    }
         
     }

     void LeftPivot()
     {
         foreach(GameObject cube in cubes)
         {
			print("x: " + cube.transform.position.x); 
			Debug.Log (cube.transform.position);
             if(cube.transform.position.x == 0.9f)
             {
                 cube.transform.parent = pivot.transform;
                 side[i] = cube;
                 i++;
             }
         }
     }

     void UpPivot()
     {
         foreach(GameObject cube in cubes)
         {
			print ("y: " + cube.transform.position.y);
             if(cube.transform.position.y == -0.4f)
             {
                 cube.transform.parent = pivot.transform;
                 side[i] = cube;
                 i++;
             }
         }

     }

     void DownPivot()
     {
         foreach(GameObject cube in cubes)
         {
			print ("y: " + cube.transform.position.y);
             if(cube.transform.position.y == -0.6f)
             {
                 cube.transform.parent = pivot.transform;
                 side[i] = cube;
                 i++;
             }
         }
     }
     
      void FrontPivot()
     {
         foreach(GameObject cube in cubes)
         {
			print ("z: " + cube.transform.position.z);	
             if(cube.transform.position.z == 2.9f)
             {
                 cube.transform.parent = pivot.transform;
                 side[i] = cube;
                 i++;
             }
         }
     }

     void BackPivot()
     {
         foreach(GameObject cube in cubes)
         {
			print ("z: " + cube.transform.position.z);
             if(cube.transform.position.z == 3.1f)
             {
                 cube.transform.parent = pivot.transform;
                 side[i] = cube;
                 i++;
             }
         }
     }

	public void resetCube(){
		scrambleIndex = 0;
		totalRotation = 0;
		Start ();
	}
	public void setAlgorithm(string[] algorithm){
		scramble = algorithm;
	}
}
