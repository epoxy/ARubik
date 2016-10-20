using UnityEngine;
using System.Collections;

public class ProgressBarScript : MonoBehaviour {
 
 	private Color tmp;
 	public int level = 0;

 	private float transparency = 0.5f;

 	public GameObject step1;
 	public GameObject step2;
 	public GameObject step3;
 	public GameObject step4;

 	public GameObject task2_1;
 	public GameObject task2_2;
 	public GameObject task3_1;
 	public GameObject task3_2;
 	public GameObject task4_1;
 	public GameObject task4_2;

 	public GameObject line1_0;
 	public GameObject line2_0;
 	public GameObject line2_1hor;
 	public GameObject line2_1ver;
 	public GameObject line2_2;
 	public GameObject line3_0;
 	public GameObject line3_1hor;
 	public GameObject line3_1ver;
 	public GameObject line3_2;
 	public GameObject line4_1hor;
 	public GameObject line4_1ver;
 	public GameObject line4_2;

 	public GameObject check1;
 	public GameObject check2;
 	public GameObject check3;
 	public GameObject check4;
 	public GameObject check2_1;
 	public GameObject check2_2;
 	public GameObject check3_1;
 	public GameObject check3_2;
 	public GameObject check4_1;
 	public GameObject check4_2;

 	

	// Use this for initialization
	void Start () {
		
        //Color tmp = step1.GetComponent<SpriteRenderer>().color;
 		//tmp.a = transparency;
 		//step1.GetComponent<SpriteRenderer>().color = tmp;

		level = 0;

 		setTransparent(step2);
 		setTransparent(step3);
 		setTransparent(step4);
 		
 		setTransparent(line1_0);
 		setTransparent(line2_0);
 		setTransparent(line3_0);

		disableSprite(check1);
		disableSprite(check2);
		disableSprite(check3);
		disableSprite(check4);
		disableSprite(check2_1);
		disableSprite(check2_2);
		disableSprite(check3_1);
		disableSprite(check3_2);
		disableSprite(check4_1);
		disableSprite(check4_2);

		disableSprite(task2_1);
		disableSprite(task2_2);
		disableSprite(line2_1hor);
		disableSprite(line2_1ver);

		disableSprite(task3_1);
		disableSprite(task3_2);
		disableSprite(line3_1hor);
		disableSprite(line3_1ver);
		disableSprite(line3_2);

		disableSprite(task4_1);
		disableSprite(task4_2);
		disableSprite(line4_1hor);
		disableSprite(line4_1ver);
		disableSprite(line4_2);

	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.RightArrow)){
            level += 3;
            upLevel();
            print("level: " + level);
		}
		*/

        if (Input.GetKeyDown(KeyCode.DownArrow)){
            level += 1;
            if(level == 14){
            	level = 0;
            }
            upLevel();
            print("level: " + level);
        }

	}

	void setTransparent(GameObject obj){
		tmp = obj.GetComponent<SpriteRenderer>().color;
 		tmp.a = transparency;
 		obj.GetComponent<SpriteRenderer>().color = tmp;
	}

	void setVisible(GameObject obj){
		tmp = obj.GetComponent<SpriteRenderer>().color;
 		tmp.a = 1f;
 		obj.GetComponent<SpriteRenderer>().color = tmp;
	}

	void enableSprite(GameObject obj){
		obj.GetComponent<SpriteRenderer>().enabled = true;
	}

	void disableSprite(GameObject obj){
		obj.GetComponent<SpriteRenderer>().enabled = false;
	}

	public void upLevel(){
		string s = level.ToString();
		switch(s){
			case "0":
				level0();
				break;
			case "1":
				level1();
				break;
			case "2":
				level2();
				break;
			case "3":
				level3();
				break;
			case "4":
				level4();
				break;
			case "5":
				level5();
				break;
			case "6":
				level6();
				break;
			case "7":
				level7();
				break;
			case "8":
				level8();
				break;
			case "9":
				level9();
				break;
			case "10":
				level10();
				break;
			case "11":
				level11();
				break;
			case "12":
				level12();
				break;
			case "13":
				level13();
				break;

		}
	}

	public void reset(){
		Start ();
	}

	void level0(){
		disableSprite(check1);
		disableSprite(check2);
		disableSprite(check3);
		disableSprite(check4);
		setTransparent(step2);
		setTransparent(step3);
		setTransparent(step4);
		setTransparent(line2_0);
		setTransparent(line3_0);
		
	}

	void level1(){
		setVisible(step2);
		setVisible(line1_0);
		enableSprite(check1);
	}

	void level2(){
		enableSprite(task2_1);
		enableSprite(task2_2);
		enableSprite(line2_1hor);
		enableSprite(line2_1ver);
		enableSprite(line2_2);
		setTransparent(task2_2);
		setTransparent(line2_2);
	}

	void level3(){
		setVisible(task2_2);
		setVisible(line2_2);
		enableSprite(check2_1);
	}

	void level4(){
		enableSprite(check2);
		enableSprite(check2_2);
	}

	void level5(){
		setVisible(step3);
		setVisible(line2_0);
		disableSprite(task2_1);
		disableSprite(task2_2);
		disableSprite(line2_1hor);
		disableSprite(line2_1ver);
		disableSprite(line2_2);
		disableSprite(check2_1);
		disableSprite(check2_2);
	}

	void level6(){
		enableSprite(task3_1);
		enableSprite(task3_2);
		enableSprite(line3_1hor);
		enableSprite(line3_1ver);
		enableSprite(line3_2);
		setTransparent(task3_2);
		setTransparent(line3_2);
	}

	void level7(){
		setVisible(task3_2);
		setVisible(line3_2);
		enableSprite(check3_1);
	}

	void level8(){
		enableSprite(check3);
		enableSprite(check3_2);
	}


	void level9(){
		setVisible(step4);
		setVisible(line3_0);
		disableSprite(task3_1);
		disableSprite(task3_2);
		disableSprite(line3_1hor);
		disableSprite(line3_1ver);
		disableSprite(line3_2);
		disableSprite(check3_1);
		disableSprite(check3_2);
	}

	void level10(){
		enableSprite(task4_1);
		enableSprite(task4_2);
		enableSprite(line4_1hor);
		enableSprite(line4_1ver);
		enableSprite(line4_2);
		setTransparent(task4_2);
		setTransparent(line4_2);
	}

	void level11(){
		setVisible(task4_2);
		setVisible(line4_2);
		enableSprite(check4_1);
	}

	void level12(){
		enableSprite(check4_2);
		enableSprite(check4);
	}

	void level13(){
		disableSprite(task4_1);
		disableSprite(task4_2);
		disableSprite(line4_1hor);
		disableSprite(line4_1ver);
		disableSprite(line4_2);
		disableSprite(check4_1);
		disableSprite(check4_2);
	}
	




}
