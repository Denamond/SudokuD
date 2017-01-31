using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechTest : MonoBehaviour {

	int result = 0;
	string list;

	List<int> fillNumv = new List<int>();
	// Use this for initialization
	void Start () {
		for (int a = 1; a < 11; a++) fillNumv.Add(a); //Creates a list of numbers from 1 to 9
		for (int i = 0; i < fillNumv.Count; i++) //shuffles the 9 numbers once each
		{
			int temp = fillNumv[i]; //store number temporarily
			int randomIndex = Random.Range(i, fillNumv.Count); //choose random number range from i to end of array
			fillNumv[i] = fillNumv[randomIndex]; //replace current value with random
			fillNumv[randomIndex] = temp; //replace random value with the stored one
		}
		fillNumv.RemoveAt(9);
		foreach(int v in fillNumv) list = list + " " + v;
		print (list);
		print (FindNumber());
	}

	int FindNumber() {
		for (int x = 1; x <=10 ; x++)
		{
			if (Check(x)) return x;
		}
		return 0;
	}

	public bool Check(int i){
		foreach (int a in fillNumv) {
			if (a == i) return false;
		}
		return true;
	}
}
