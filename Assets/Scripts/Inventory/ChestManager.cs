using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestManager : MonoBehaviour {

	// UI
	public GameObject chestPanel;
	GameObject slotPanel;
	EventLogger eventLogger;

	public GameObject inventoryItem;

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

	private void ClearPanel() {
		foreach (GameObject slot in slots) {
			while(slot.transform.childCount != 0){
				DestroyImmediate(slot.transform.GetChild(0).gameObject);
			}
		}
	}

	public void DisplayChest(int id) {
		Chest chest = level.chests[id];

		for (int i = 0; i < chest.items.Length; i++) {
			Item itemToAdd = inventory.database.FetchItemByID (id);
			GameObject itemObject = Instantiate (chestItem);
			itemObject.GetComponent<ItemData>().item = itemToAdd;
			itemObject.GetComponent<ItemData>().slot = i;
			itemObject.transform.SetParent (slots [i].transform);
			itemObject.GetComponent<Image> ().sprite = itemToAdd.Sprite;
			
			// Propperly position item on slot image
			RectTransform itemRectTrasform = itemObject.GetComponent<RectTransform> ();
			itemRectTrasform.offsetMin = Vector2.zero;
			itemRectTrasform.offsetMax = Vector2.zero;

			itemObject.transform.parent = slots[i].transform;

		}

	}
	// public void AddItem(int id) {
	// 	Item itemToAdd = inventory.database.FetchItemByID (id);

	// 	if (itemToAdd.Stackable && CheckIfItemIsInInventory (itemToAdd)) {
	// 		for (int i = 0; i < items.Count; i++) {
	// 			if (items [i].ID == id) {
	// 				ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
	// 				data.amount++;
	// 				data.transform.GetChild (0).GetComponent<Text> ().text = (data.amount + 1).ToString();
	// 				break;
	// 			}
	// 		}
	// 	} else {

	// 		for (int i = 0; i < items.Count; i++) {

	// 			// Empty slot
	// 			if (items [i].ID == -1) {
	// 				items [i] = itemToAdd;
	// 				GameObject itemObject = Instantiate (inventoryItem);
	// 				itemObject.GetComponent<ItemData>().item = itemToAdd;
	// 				itemObject.GetComponent<ItemData>().slot = i;
	// 				itemObject.transform.SetParent (slots [i].transform);
	// 				itemObject.GetComponent<Image> ().sprite = itemToAdd.Sprite;
			
	// 				// Propperly position item on slot image
	// 				RectTransform itemRectTrasform = itemObject.GetComponent<RectTransform> ();
	// 				itemRectTrasform.offsetMin = Vector2.zero;
	// 				itemRectTrasform.offsetMax = Vector2.zero;

	// 				itemObject.name = itemToAdd.Title;

	// 				//Break out of the loop after adding an item
	// 				break;
	// 			}

	// 		}
	// 	}
	// }
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Toggles wheter the chest panel is visible or not 
	public void SetOpen(bool open) {
		chestPanel.GetComponent<Animator>().SetBool("IsDisplayed", open);
	}
}
