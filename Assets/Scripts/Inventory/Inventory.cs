using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	GameObject inventoryPanel;
	GameObject slotPanel;

	ItemDatabase database;

	// Prefabs
	public GameObject inventorySlot;
	public GameObject inventoryItem;


	int slotCount;
	public List<Item> items = new List<Item>();
	public List<GameObject> slots = new List<GameObject>();

	void Start() {

		database = GetComponent<ItemDatabase> ();

		slotCount = 20;
		inventoryPanel = GameObject.Find ("Inventory Panel");
		slotPanel = inventoryPanel.transform.FindChild ("Slot Panel").gameObject;


		for (int i = 0; i < slotCount; i++) {

			items.Add (new Item ());
			slots.Add (Instantiate (inventorySlot));
			slots [i].transform.SetParent(slotPanel.transform, false);
		}

		AddItem (0);
		AddItem (1);
	}

	public void AddItem(int id) {
		Item itemToAdd = database.FetchItemByID (id);
		for (int i = 0; i < items.Count; i++) {

			// Empty slot
			if (items [i].ID == -1) {
				items [i] = itemToAdd;
				GameObject itemObject = Instantiate (inventoryItem);
				itemObject.transform.SetParent (slots[i].transform);
				itemObject.GetComponent<Image> ().sprite = itemToAdd.Sprite;
			
				// Propperly position item on slot image
				RectTransform itemRectTrasform = itemObject.GetComponent<RectTransform> ();
				itemRectTrasform.offsetMin = Vector2.zero;
				itemRectTrasform.offsetMax = Vector2.zero;

				itemObject.name = itemToAdd.Title;

				//Break out of the loop after adding an item
				break;
			}

		}
	}
}
