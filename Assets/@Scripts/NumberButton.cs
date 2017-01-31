using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour {

	//NumberButton script created by Luis Felipe Madiedo
	public bool isActive; //Is this button active?
	public bool changeNum = false; //Stores if the number was changed (player made a change)

	public Transform activeButton; //Current active button
	public Transform mainScript; //Where main scripts are hold

	public Color BaseText; //Dark gray color for correct numbers
	public Color TempText; //Blue color for "could be right but not exact number"
	public Color WrongText; //Red Color for wrong numbers

	void Update () {

		//Place numbers with keypad
		if (activeButton != null){ //Won't do anything unless there is a transform on activeButton otherwise it shows an error
			
			if (Input.GetKeyUp(KeyCode.Keypad1)) //We use keypad
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "1") changeNum = true; //This changes the changenum if the number to be set is different than currently placed
				activeButton.GetComponentInChildren<Text>().text = "1"; //This changes the text to the keypad number used
				CompareResult(); //Calls function to compare results
			}
			if (Input.GetKeyUp(KeyCode.Keypad2))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "2") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "2";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad3))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "3") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "3";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad4))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "4") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "4";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad5))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "5") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "5";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad6))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "6") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "6";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad7))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "7") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "7";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad8))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "8") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "8";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad9))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "9") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "9";
				CompareResult();
			}
			if (Input.GetKeyUp(KeyCode.Keypad0))
			{	
				if (activeButton.GetComponentInChildren<Text>().text != "") changeNum = true; 
				activeButton.GetComponentInChildren<Text>().text = "";
				CompareResult();
			}
		}
	}

	public void PressButton(string bt){ //This code works with the buttons on the screen from 0 to 9, 0 making it empty
		if (activeButton != null){			
			activeButton.GetComponentInChildren<Text>().text = bt;
			CompareResult(); 
		}
	}
		
	public void ActiveOff(){
		activeButton = null;
	}

	public void CompareResult(){ //Compares results with all peers if no conflict found make it temp color (blue)
		if (ComparePeersT(activeButton)) { //Compares the current space with all the peers to check if no conflicts are found
			activeButton.GetComponentInChildren<Text>().color = TempText;
			activeButton.GetComponent<ButtonScript>().correct = true; //Make temporarily true
		}
		else
		{
			activeButton.GetComponentInChildren<Text>().color = WrongText; //If there is conflict then make it and peers wrong colored (red)
			activeButton.GetComponent<ButtonScript>().correct = false; //if wrong then make false
		}
		if (activeButton.GetComponentInChildren<Text>().text == activeButton.GetComponent<ButtonScript>().value){ //If no conflict AND also the correct number then make black
			activeButton.GetComponentInChildren<Text>().color = BaseText;
			activeButton.GetComponent<ButtonScript>().correct = true;
		}
		if (changeNum) ComparePeersX(activeButton); //If a change is made of number then make peers adjust their colors
		ComparePeersW(activeButton);
		changeNum = false; //Make changeNum false for next change
		if (mainScript.GetComponent<PuzzleStart>().CheckVictory()) mainScript.GetComponent<PuzzleStart>().Victory(); //Check victory after each change
	}

	public bool ComparePeersT(Transform buttonT){ //Quick function to compare values with each peer as a bool, if there is a single conflict then returns false otherwise true
		foreach ( Transform Peered in buttonT.GetComponent<ButtonScript>().Peers )
		{
			if (buttonT.GetComponentInChildren<Text>().text == Peered.GetComponentInChildren<Text>().text)	return false;
		}
		return true;
	}

	public void ComparePeersX(Transform buttonX){ //Function to check peers and their colors, works same as the chosen space
		foreach ( Transform Peered in buttonX.GetComponent<ButtonScript>().Peers )
		{
			if (ComparePeersT(Peered)) {
				Peered.GetComponentInChildren<Text>().color = TempText;
				Peered.GetComponent<ButtonScript>().correct = true;
			}
			else
			{
				Peered.GetComponentInChildren<Text>().color = WrongText;
				Peered.GetComponent<ButtonScript>().correct = false;
			}
			if (Peered.GetComponentInChildren<Text>().text == Peered.GetComponent<ButtonScript>().value){
				Peered.GetComponentInChildren<Text>().color = BaseText;
				Peered.GetComponent<ButtonScript>().correct = true;
			}
		}
	}

	public void ComparePeersW(Transform buttonW){ //Quick function to compare values with each peer as a bool, if there is a single conflict then returns false otherwise true
		foreach ( Transform Peered in buttonW.GetComponent<ButtonScript>().Peers )
		{
			if (buttonW.GetComponentInChildren<Text>().text == Peered.GetComponentInChildren<Text>().text){
				Peered.GetComponentInChildren<Text>().color = WrongText;
			}
		}
	}
}
