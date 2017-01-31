using System.Collections;
using System.Collections.Generic; //Because we use lists
using UnityEngine;
using UnityEngine.UI; //Because we use buttons

public class PuzzleStart : MonoBehaviour {

	//PuzzleStart script created by Luis Felipe Madiedo

	//This is the main code, the process I decided to make so that the sudoku works is to first make it so that the game provides randomly generated puzzle soulitons then taking out some of the spaces so the player can fill them.
	/*First step was to generate the ranbom solution which would start by placing the numbers row by row, in order to do this the script takes each spot and sets a list of valid numbers on each so first row all spaces will have
	1-9 as valid numbers but then the next would have one missing number from the valid. This worked well until the conflicts started or list yielded no valid numbers, to solve this I divided the number placing by placing the
	first 3 numbers in the row then the 6 remaining ones, it would place the first 3 valid numbers and shuffle the remaining 6 for results, if none are found then shuffle the first 3. IF by any chance there was no possible arrangement
	then the whole process is started again. The spaces are then taken out with patterns done in PuzzlePrepare script, these depend on difficulty chosen. I put for now 3 patterns per difficulty.*/

	public Button[] gameButton = new Button [81]; //Made to store all 81 numbers, done manually through editor could be done automatically with a loop by assigning a transform with all of the buttons common parent

	List<string> fillNum = new List<string>(); //The list we use on multiple ocassions, this is supposed to hold nubmers from 1 to 9 in random order
	List<string> [] possibleNum = new List <string> [9]; //List holding possible valid numbers for each space on the entire row

	public Transform currentBut; //Sets a transform to compare
	public Transform compareBut; //Sets a transform to compare
	public Transform currentVal; //Sets a transform to compare
	public Transform compareVal; //Sets a transform to compare
	public Transform startMenu; //Transform that holds the start screen, we switch this from active to inactive
	public Transform victoryMenu; //Transform that holds the victory screen, we switch this from active to inactive
	public Transform timerK; //Transform that holds the timer to access its code

	public Color BaseText; //Dark gray color for correct numbers

	public Text DiffText; //Text that holds the UI text difficulty shown on upper right corner
	public Text TTimeS; //Text that holds the UI text seconds at victory screen
	public Text TTimeM; //Text that hold the UI text minutes at victory screen
	public Text UDiff; //Text that holds the UI text difficulty at victory screen

	public string [] DiffString = new string[5]; //Holds the difficulty strings of "beginner" "easy" "medium" "hard" and "expert"

	int [] possibleAmount = new int[9]; //Will store the current possible number position of list to be used
	int [] maxAmount = new int[9]; //stores the maximum number of positions on list

	bool AddValue = false; //Used to check if there is no conflict and make number valid
	bool ValueSuccess = false; //Used to check if any of the list created of valid numbers has at least one valid number
	bool StartSuccess = false; //Checks if there was an issue with the setting of the last 6 numbers
	bool ThreeSuccess = false; //Successfully placed first 3 numbers
	bool SixSuccess = false; //Successfully placed last 6 numbers
	bool CompareSuccess = false; //Used to check if comparing between a space and its 20 peers there is any conflic
	bool LoopOut = false;
	bool testt = false;

	int EXIT = 0; //Value that helps prevent multiple calculations
	int cLine = 0; //Current line that the script is working on goes through 0 to 8
	int Ex1 = 0; //Value that helps prevent multiple loops
	int Ex2 = 0; //Value that helps prevent multiple loops
	int Difficu = 0; //Hold the chosen difficulty from 0 - 4 begginer-easy-normal-hard-expert

	void Start () 
	{
		for (int li = 0; li < 9; li++){ //Generate list array and all other arrays
			possibleNum[li] = new List<string>();
			possibleAmount[li] = 0;
			maxAmount[li] = 0;
		}
	}

	public void CreatePuzzle(){
		ClearPuzzle(); //just in case clear the grid
		while (!testt){ //Populate grid
			testt = true;
			GoThroughGrid();
			if (!ConfirmPuzzle()) testt = false;
		}
		testt = false;
		AssignNumber();
	}

	public void GoThroughGrid(){ //Main code that goes through each space row by row
		while (!LoopOut){
			LoopOut = true; //We assume we will not need looping
			ClearPuzzle(); //We clear the puzzle
			for (int gd = 0; gd < 9; gd++) //This is done 9 times, once per row/line
			{
				while (!ThreeSuccess) {
					GoThroughLineX(); //Go though the row/line that sets the first 3 numbers
					Ex1++; //loop counter
					if (Ex1 > 20) { //If loop exceeds 20 then escape
						ThreeSuccess = true;
						SixSuccess = true;
						gd = -1;
						LoopOut = false;
						ClearPuzzle();
					}
				} //If reaches this point then ThreeSuccess was true and the first three numbers are placed
				ThreeSuccess = false; //We make it false for next line
				while (!SixSuccess) { 
					GoThroughLineZ(); //Go through row/line that sets the last 6 numbers
					Ex2++; //loop counter 2
					if (Ex2 > 20 || Ex1 > 20) { //If either counter is higher than 20 then escape
						ThreeSuccess = true;
						SixSuccess = true;
						gd = -1;
						LoopOut = false;
						ClearPuzzle();
					}
				} //If reaching this point then means that the 6 numbers at the end were also successful
				SixSuccess = false; //We make the bool false for next row/line
				Ex1 = 0; //We make the loop counters 0
				Ex2 = 0;
			}
		}
		LoopOut = false;
		/*if (ConfirmPuzzle()) {}
		else print ("NOT OK");*/
	}

	public void CreateList()
	{ 
		fillNum.Clear(); //Clear list to avoid issues
		for (int a = 1; a < 10; a++) fillNum.Add(""+a); //Creates a list of numbers from 1 to 9
		for (int i = 0; i < fillNum.Count; i++) //shuffles the 9 numbers once each
		{
			string temp = fillNum[i]; //store number temporarily
			int randomIndex = Random.Range(i, fillNum.Count); //choose random number range from i to end of array
			fillNum[i] = fillNum[randomIndex]; //replace current value with random
			fillNum[randomIndex] = temp; //replace random value with the stored one
		}
	}
		
	public void GoThroughLineX() //Goes through the first 3 values of the current cLine horizontal line
	{
		ValueSuccess = true; //We assume the values will go through
		for (int ho = 0; ho < 3; ho++) //We choose each of the 3 values, make a random ordered list of numbers from 1 to 9 and run the code to get the valid numbers
		{
			currentBut = gameButton[ho + (cLine*9)].transform;
			CreateList();
			GetRowPListX(currentBut, ho);
		}
		if (!ValueSuccess) { //If the code to get valid numbers does not change value of bool ValueSuccess we continue to next part otherwise it clears the lists and does not send a confirmation to initial function
			//print ("Threefail");
			ClearList (0, 3);
		}
		else{
			ThreeFirstRow(); // If we get valid numbers then we run the function to set those numbers
			ThreeSuccess = true; // This is the bool that confirms the initial function to proceed
		}
	}

	public void GoThroughLineZ() //Goes through the last 6 values of the current cLine horizontal line
	{		
		ValueSuccess = true; // We use same bool to confirm
		for (int ho = 3; ho < 9; ho++) //We choose each of the 6 values, make a random ordered list of numbers from 1 to 9 and run the code to get the valid numbers 
		{
			currentBut = gameButton[ho + (cLine*9)].transform;
			CreateList();
			GetRowPListX(currentBut, ho);
		}
		if (!ValueSuccess) { //If the code to get valid numbers does not change value of bool ValueSuccess we continue to next part otherwise it clears the lists and does not send a confirmation to initial function
			ClearList (3, 9);
			TFRIncrease(); //If there is no success this will change the number of possible ones from the first 3, so for example the first three numbers could be (3-5|1-9|7-6) it will go like: 596->597->516->517->396->397->316->317
			ThreeFirstRow(); //After increasing the number run once again the function to apply them to the grid
		}
		else {
			SixLastRow(); //We run the code for the last six numbers here
			if (!StartSuccess){ //This checks for another possible conflict with the amount of loops the function assigning numbers can make, there is a chance for unsovable positioning so this will trigger when enough loops happen
				ClearList (3, 9);
				TFRIncrease(); //If there is no success this will change the number of possible ones from the first 3, so for example the first three numbers could be (3-5|1-9|7-6) it will go like: 596->597->516->517->396->397->316->317
				ThreeFirstRow(); //After increasing the number run once again the function to apply them to the grid
			}
			else{
				ClearList (0, 9); //If succesful in creating the line and assigning numbers then clear the entire list
				if (cLine<8) cLine++; //Increase horizontal line worked on by 1 until8 (0 - 8)
				else cLine = 0; //When 8 instead of increasing return to 0
				SixSuccess = true; //Confirms initial script that the line was successfully placed
			}
		}
	}

	public void GetRowPListX(Transform buttonT, int sho) //Main task is to find the possible numbers that a grid space can have
	{
		
		for (int addo = 0; addo < 9; addo++) //For made to check each of the 9 randomly organized numbers of the list
		{
			AddValue = true; //We assume that the number will be added unless conflict is found

			foreach ( Transform Peered in buttonT.GetComponent<ButtonScript>().Peers ) //All buttons have 20 peers, this does the following script 20 times
			{
				if (fillNum[addo] == Peered.GetComponentInChildren<Text>().text) //Checks if there is any conflict with peers in regards to text
				{
					AddValue = false; //If conflict then states that the value is invalid and is not added to the possibleNum list.
				}
			}
			if (AddValue) { 
				possibleNum[sho].Add(""+fillNum[addo]); //If addvalue is true means that the number has no conflict and it is added to the posibbleNum list of valid numbers for that spot
				maxAmount[sho]++; //Confirming a number increases the max amount (1-9) of valid numbers this is useful bit later
			}
		}
		if (maxAmount[sho] == 0) ValueSuccess = false; //MaxAmount being 0 means there is no valid numbers which should not happen often, however it will still send information that the attempt failed
	}

	public void ThreeFirstRow()
	{
		while (!CompareSuccess)
		{
			for (int q = 0; q < 3; q++)
			{
				gameButton[q + (cLine*9)].transform.GetComponentInChildren<Text>().text = possibleNum[q][possibleAmount[q]]; //Make the line text be the text number stored on that position
			}

			if( ComparePeersT(gameButton[0 + (cLine*9)].transform) 
				&& ComparePeersT(gameButton[1 + (cLine*9)].transform) 
				&& ComparePeersT(gameButton[2 + (cLine*9)].transform) ) CompareSuccess = true; //compares all peers for each number, if all 3 are true then proceed
			else
			{
				TFRIncrease(); //This part of this script is called on other parts of the script so a sepparate function was made
			}
		}
		CompareSuccess = false; //Made false for following line
	}

	public void TFRIncrease() //Should simply increase the values taken from each of the three lists: 000|001|010|011|100|101|110|111 For example if each had only 2 values
	{
		if ((possibleAmount[2]+1) < maxAmount[2]) //If maxAmount not reached then increase the number of the possible amount used for that list
		{
			possibleAmount[2]++;
		}
		else //If maxAmount reached then return number of possible amount for that list to 0 and increase the next one (firsts checks if possible) 
		{
			possibleAmount[2] = 0;
			if ((possibleAmount[1]+1) < maxAmount[1]) 
			{
				possibleAmount[1]++;
			}
			else 
			{
				possibleAmount[1] = 0;
				if ((possibleAmount[0]+1) < maxAmount[0]) 
				{
					possibleAmount[0]++;
				}
				else 
				{
					possibleAmount[0] = 0;
					for (int q = 0; q < 3; q++) //It is possible to reach this point before the amount of loops for escape are reached, we will send a "unsuccessful" bool in this case and clean the list to avoid errors
					{
						gameButton[q + (cLine*9)].transform.GetComponentInChildren<Text>().text = "0";
					}
				}
			}
		}
	}

	public void SixLastRow() //Main task is to find the possible numbers the last 6 spots of the grid can have
	{
		StartSuccess = true; //Assuming everthing goes well
		EXIT = 0; //This is to prevent anymore loops than necessary
		while (!CompareSuccess) //As long as there is no success keep the loop
		{
			for (int q = 3; q < 9; q++) //We are using the last 6 numbers
			{
				gameButton[q + (cLine*9)].transform.GetComponentInChildren<Text>().text = possibleNum[q][possibleAmount[q]]; //Assigns the last 6 numbers to the possible valid value on the list
			}

			if( ComparePeersT(gameButton[3 + (cLine*9)].transform)
				&& ComparePeersT(gameButton[4 + (cLine*9)].transform)
				&& ComparePeersT(gameButton[5 + (cLine*9)].transform)
				&& ComparePeersT(gameButton[6 + (cLine*9)].transform)
				&& ComparePeersT(gameButton[7 + (cLine*9)].transform)
				&& ComparePeersT(gameButton[8 + (cLine*9)].transform)) CompareSuccess = true; //Similar to code above checks 6 spots to see if there is conflicts with peers, if true then proceed
			else
			{
				EXIT++; //entering else means there is a conflict, EXIT will keep track of the loops and if above certain treshold then "escapes" and sends an unsuccessful bool to initial function
				if ((possibleAmount[8]+1) < maxAmount[8]) 
				{
					possibleAmount[8]++;
				}
				else 
				{
					possibleAmount[8] = 0;
					if ((possibleAmount[7]+1) < maxAmount[7]) 
					{
						possibleAmount[7]++;
					}
					else 
					{
						possibleAmount[7] = 0;
						if ((possibleAmount[6]+1) < maxAmount[6]) 
						{
							possibleAmount[6]++;
						}
						else 
						{
							possibleAmount[6] = 0;
							if ((possibleAmount[5]+1) < maxAmount[5]) 
							{
								possibleAmount[5]++;
							}
							else 
							{
								possibleAmount[5] = 0;
								if ((possibleAmount[4]+1) < maxAmount[4]) 
								{
									possibleAmount[4]++;
								}
								else 
								{
									possibleAmount[4] = 0;
									if ((possibleAmount[3]+1) < maxAmount[3]) 
									{
										possibleAmount[3]++;
									}
									else 
									{
										for (int q = 3; q < 9; q++) //It is possible to reach this point before the amount of loops for escape are reached, we will send a "unsuccessful" bool in this case and clean the list to avoid errors
										{
											gameButton[q + (cLine*9)].transform.GetComponentInChildren<Text>().text = "0";
										}
										CompareSuccess = true;
										StartSuccess = false;
									}
								}
							}
						}
					}
				}
				if (EXIT > 1000) {
					for (int q = 3; q < 9; q++) //Reaching these many amount of loops is considered unsuccessfuly and thus the list is cleaned and bools stating the failure is send
					{
						gameButton[q + (cLine*9)].transform.GetComponentInChildren<Text>().text = "0";
					}
					CompareSuccess = true;
					StartSuccess = false;
				}
			}
		}
		CompareSuccess = false; //Made false for next line
	}

	public void ClearPuzzle(){ //Quick function to clear the entire grid and make it 0, also at the end makes cLine 0 so that current horizontal line worked on backs to 0
		for (int c = 0; c < 81; c++){
			gameButton[c].GetComponentInChildren<Text>().text = "0";
		}
		cLine = 0;
	}

	public bool ConfirmPuzzle(){ //Quick function to check if any of the numbers is 0, if any is 0 then sends a false bool back
		for (int cu = 0; cu < 81; cu++){
			if (gameButton[cu].GetComponentInChildren<Text>().text == "0") return false;
			gameButton[cu].GetComponent<ButtonScript>().editable = false;

			ColorBlock cb = gameButton[cu].GetComponent<Button>().colors;
			cb.normalColor = Color.white;
			gameButton[cu].GetComponent<Button>().colors = cb;

		}
		return true;
	}

	public void ClearList(int a, int b){ //Simple function to clear lists made, this is done here and there to avoid conflicts or lists becoming too large and with many valid numbers
		for (int lc = a; lc <b; lc++) {
			possibleNum[lc] = new List<string>();
			possibleNum[lc].Clear();
			possibleAmount[lc] = 0; //Possible numbers are cleared
			maxAmount[lc] = 0; //Max amount of numbers are cleared too
		}
	}

	public bool ComparePeersT(Transform buttonT){ //Quick function to compare values with each peer as a bool, if there is a single conflict then returns false otherwise true
		foreach ( Transform Peered in buttonT.GetComponent<ButtonScript>().Peers )
		{
			if (buttonT.GetComponentInChildren<Text>().text == Peered.GetComponentInChildren<Text>().text)	return false; //Current space has same number as a peer? Return false otherwise true.
		}
		return true;
	}

	public void AssignNumber(){ //Quick function to assign the found numbers as the correct values to use
		for (int c = 0; c < 81; c++){
			gameButton[c].GetComponent<ButtonScript>().value = gameButton[c].GetComponentInChildren<Text>().text; //Current text will be copied to value (correct value)
		}
	}

	public void PuzzlePrepareN(){ //Function that according to difficulty chooses a patter at random
		int randomPattern = Random.Range(0, 2); //Random pattern from 1 to 3
		for (int zv = 0; zv < 9; zv++){
			for (int zh = 0; zh < 9; zh++){
				if (this.GetComponent<PuzzlePrepare>().difficultyDx[Difficu,randomPattern,zv,zh] == 1){ //From chosen pattern compares if the position is 1 or 0 (1 is non editable static number and 0 is editable and hidden)
					ColorBlock cb = gameButton[zh + (zv*9)].GetComponent<Button>().colors; //Create a colorblock to change Button normal color, cannot be done on a single line
					cb.normalColor = gameButton[zh + (zv*9)].GetComponent<ButtonScript>().Noneditable; 
					gameButton[zh + (zv*9)].GetComponent<Button>().colors = cb;
					gameButton[zh + (zv*9)].GetComponent<ButtonScript>().correct = true; //We set these numbers as correct as they are part of the solution initially found
				}
				else{
					gameButton[zh + (zv*9)].GetComponentInChildren<Text>().text = ""; //We hide this numbers now that we know which ones we want the player to find
					gameButton[zh + (zv*9)].GetComponent<ButtonScript>().editable = true; //We make them editable so that player can set a number on them
				}
			}
		}
	}

	public void Victory(){ //Quick function to assign the found numbers as the correct values to use
		timerK.GetComponent<TimerHandler>().Store();
		victoryMenu.gameObject.SetActive(true);
		TTimeM.text = timerK.GetComponent<TimerHandler>().MinutesS;
		TTimeS.text = timerK.GetComponent<TimerHandler>().SecondsS;
	}

	public bool CheckVictory(){ //Quick function to compare values with each peer as a bool, if there is a single conflict then returns false otherwise true
		for (int c = 0; c < 81; c++){
			if (gameButton[c].GetComponent<ButtonScript>().correct != true) return false; //Current text will be copied to value (correct value)
		}
		return true;
	}

	public void SetDiff(int dif){ //Set difficulty function
		for (int c = 0; c < 81; c++){	//This will set the peers for all 81 spaces as well as make their colors dark gray
			gameButton[c].GetComponent<ButtonScript>().SetPeers();
			gameButton[c].GetComponentInChildren<Text>().color = BaseText;
		}
		Difficu = dif; //Button pressed on difficulty menu decides the int that difficult is going to use 0-4
		CreatePuzzle();
		PuzzlePrepareN();
		timerK.GetComponent<TimerHandler>().activeTime = true; //Make timer start
		DiffText.text = DiffString[dif];
		startMenu.gameObject.SetActive(false);
		victoryMenu.gameObject.SetActive(false);
	}

	public void ChangeDiff(){ //Just makes the difficulty menu appear again
		startMenu.gameObject.SetActive(true);
	}

	public void ClearEdit(){ //Clean the editable spaces of the current puzzle of many numbers are wrong
		for (int c = 0; c < 81; c++){
			gameButton[c].GetComponentInChildren<Text>().color = BaseText;
			if (gameButton[c].GetComponent<ButtonScript>().editable == true) {
				gameButton[c].GetComponentInChildren<Text>().text = "";
				gameButton[c].GetComponent<ButtonScript>().correct = false;
			}
		}
	}

	public void ResolvePuzzle(){ //Solves current puzzle
		for (int c = 0; c < 81; c++){
			gameButton[c].GetComponentInChildren<Text>().color = BaseText;
			if (gameButton[c].GetComponent<ButtonScript>().editable == true) {
				gameButton[c].GetComponentInChildren<Text>().text = ""+gameButton[c].GetComponent<ButtonScript>().value; 
			}
			timerK.GetComponent<TimerHandler>().activeTime = false; //make timer stop
		}
	}

	public void NewPuzzle(){ //Menu button that makes new puzzle
		CreatePuzzle();
		PuzzlePrepareN();
		timerK.GetComponent<TimerHandler>().Reset(); //Timer is reset to 0
		timerK.GetComponent<TimerHandler>().activeTime = true; //Timer starts again
	}
}
