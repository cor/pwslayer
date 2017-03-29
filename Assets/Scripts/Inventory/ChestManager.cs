using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour {

	// UI
	public GameObject chestPanel;
	GameObject slotPanel;
	EventLogger eventLogger;

	public List<GameObject> slots = new List<GameObject>();
	
	// Prefabs
	public GameObject chestSlot;
	public GameObject chestItem;

	// Places to transfer items to
	Inventory inventory;
	Level level;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		level = GameObject.Find("Level").GetComponent<Level>();
		eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();

		InitializePanel();
	}

	void InitializePanel() {
		int slotCount = 16;
		slotPanel = chestPanel.transform.FindChild ("Slot Panel").gameObject;


		for (int i = 0; i < slotCount; i++) {
			slots.Add(Instantiate(chestSlot));
			slots[i].transform.SetParent(slotPanel.transform, false);	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Toggles wheter the chest panel is visible or not 
	public void SetOpen(bool open) {
		chestPanel.GetComponent<Animator>().SetBool("IsDisplayed", open);
	}
}
