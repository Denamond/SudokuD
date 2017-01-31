using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerHandler : MonoBehaviour {

	//Timerhandler script created by Luis Felipe Madiedo
	public Text Seconds; //Text showing seconds 00
	public Text Minutes; //Text showing minutes 000

	public string SecondsS; //Store seconds when required to show at end screen
	public string MinutesS; //Store minutes when required to shot at end screen

	private float time; //Keeps time with Time.deltatime per frame

	public bool activeTime = false;
		
	// Update is called once per frame
	void Update () {
		if (activeTime){ //Check if if active
			time += Time.deltaTime;
			Minutes.text = Mathf.Floor(time / 60).ToString("000");
			Seconds.text = Mathf.Floor(time % 60).ToString("00");
		}
	}

	public void Reset() {
		time = 0;
		MinutesS = "000";
		SecondsS = "00";
		Minutes.text = "000";
		Seconds.text = "00";
	}

	public void Store() {
		MinutesS = Minutes.text;
		SecondsS = Seconds.text;
	}
}
