using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Text text = GetComponent<Text>();
		int health = GameObject.FindWithTag("Player").GetComponent<Player>().health;
		text.text = "HP: " + health + "/100";
		
	}
}
