using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	//ButtonScript script created by Luis Felipe Madiedo
	public bool isActive; //Is this button active?
	public int horPos; //Horizontal Position of button (1-9)
	public int verPos; //Vertical Position of button (1-9)
	public string value = "0"; //True value from 1 to 9
	public bool shown = false; //if shown or not
	public bool correct = false; //when hidden will turn to false, and only turn true if number is shown and equals to value
	public bool editable = false; //space can have its number edited or not

	public Transform thisZone; //At start checks for parent of this button and defines the zone
	public Transform fullGrid; //Parent of parent of this button, where all buttons are held
	public Transform currentUse; //Sets the transform that is currently being checked
	public Transform numberA; //Holds the number input script

	public Color Base; //Base color of white
	public Color Shared; //Color change when sharing a position or zone with selected part of grid
	public Color Active; //Selected part of grid color
	public Color Noneditable; //The color of the spaces that cannot be changed

	public List<Transform> Peers = new List<Transform>(); //Stores the 20 peers each space has

	void Start () { 
		thisZone = this.transform.parent; //This space zone from the 9 availables is defined by their parent
		fullGrid = thisZone.transform.parent; //The grandparent of the current space
	}
		
	public void SetPeers(){ //Quick code to assign peers to this space
		for (int zv = 0; zv < 9; zv++){
			for (int zh = 0; zh < 9; zh++){
				currentUse = fullGrid.transform.GetChild(zv).transform.GetChild(zh); //According to grandparent and parent choose the space to compare
				if (currentUse != this.transform ) //If space is not same as this then...
				{
					if (this.horPos == currentUse.GetComponent<ButtonScript>().horPos //If space horizontal position is same as this OR
						|| this.verPos == currentUse.GetComponent<ButtonScript>().verPos // If space vertical position is same as this OR
						|| this.thisZone == currentUse.GetComponent<ButtonScript>().thisZone) //If they share parents (zones)
						Peers.Add(currentUse.transform); //Add the space to the peers
				}
			}
		}
	}

	public void ActiveToggle(){ //This takes care of changing the color of grids when clicked
		if (this.editable) //Only works when this space has been marked as editable
		{
			isActive = !isActive; //If active deactivate if deactivated activate
			for (int zov = 0; zov < 9; zov++)
			{
				for (int zoh = 0; zoh < 9; zoh++)
				{
					currentUse = fullGrid.transform.GetChild(zov).transform.GetChild(zoh); //Choose space based on for numbers (parent and grandparent)
					currentUse.GetComponent<Image>().color = Base; //make all spaces white or base color

					if (currentUse != this.transform ) currentUse.GetComponent<ButtonScript>().isActive = false; //Makes all other grid spaces inactive (not chosen)

					if (isActive)
					{ //Compares if the current button and the currentUse one have the same Horizontal Position, Vertical Position or Zone Parent then changes color to shared if so.
						if (this.horPos == currentUse.GetComponent<ButtonScript>().horPos 
							|| this.verPos == currentUse.GetComponent<ButtonScript>().verPos 
							|| this.thisZone == currentUse.GetComponent<ButtonScript>().thisZone) 
							currentUse.GetComponent<Image>().color = Shared;
					}
				}
			}
			if (isActive) {
				this.GetComponent<Image>().color = Active; //Cahnges current chosen grid part to active color
				numberA.GetComponent<NumberButton>().activeButton = this.transform;
			}
			else {
				numberA.GetComponent<NumberButton>().ActiveOff(); //Better to call function in case something is needed before de-assigning the activeButton
			}
		}
	}
}
