using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour {

	// UI
	public GameObject chestPanel;
	GameObject slotPanel;
	EventLogger eventLogger;

	// Places to transfer items to
	Inventory inventory;
	Level level;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		level = GameObject.Find("Level").GetComponent<Level>();
		eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Toggles wheter the chest panel is visible or not 
	public void SetOpen(bool open) {
		chestPanel.SetActive(open);
	}
}
