using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	// UI
	GameObject inventoryPanel;
	GameObject slotPanel;
	
	// Prefabs
	public GameObject inventorySlot;
	public GameObject equipedItemSlot;
	public GameObject inventoryItem;
	public Canvas canvas;
	

	ItemDatabase database;

	int slotCount;
	public List<Item> items = new List<Item>();
	public List<GameObject> slots = new List<GameObject>();

	public int equipedItemSlotID = 0; 

	void Start() {

		database = GetComponent<ItemDatabase> ();

		slotCount = 20;
		inventoryPanel = GameObject.Find ("Inventory Panel");
		slotPanel = inventoryPanel.transform.FindChild ("Slot Panel").gameObject;


		for (int i = 0; i < slotCount; i++) {

			items.Add (new Item ());
			slots.Add (Instantiate (inventorySlot));
			slots[i].GetComponent<InventorySlot>().id = i;
			slots [i].transform.SetParent(slotPanel.transform, false);
		}

		AddItem (0);
	}


	public void AddItem(int id) {
		Item itemToAdd = database.FetchItemByID (id);

		if (itemToAdd.Stackable && CheckIfItemIsInInventory (itemToAdd)) {
			for (int i = 0; i < items.Count; i++) {
				if (items [i].ID == id) {
					ItemData data = slots [i].transform.GetChild (0).GetComponent<ItemData> ();
					data.amount++;
					data.transform.GetChild (0).GetComponent<Text> ().text = (data.amount + 1).ToString();
					break;
				}
			}
		} else {

			for (int i = 0; i < items.Count; i++) {

				// Empty slot
				if (items [i].ID == -1) {
					items [i] = itemToAdd;
					GameObject itemObject = Instantiate (inventoryItem);
					itemObject.GetComponent<ItemData>().item = itemToAdd;
					itemObject.GetComponent<ItemData>().slot = i;
					itemObject.transform.SetParent (slots [i].transform);
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

	// Equip an item and show it in the slot next to the inventory icon
	public void EquipItem(int slotId) {

		equipedItemSlotID = slotId;
		
		// if the slot isn't empty
		if (items[slotId].ID != -1) {
			equipedItemSlot.transform.GetChild(0).GetComponent<Image>().sprite = items[equipedItemSlotID].Sprite;
			equipedItemSlot.transform.GetChild(0).GetComponent<Image>().color = Color.white;
		} else {
			equipedItemSlot.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
		}

		GameObject highlight = GameObject.Find("Equiped Item Highlight");
		GameObject itemSlot = slots[equipedItemSlotID];
		highlight.transform.position = itemSlot.transform.position + new Vector3(itemSlot.GetComponent<RectTransform>().rect.width * canvas.scaleFactor / 2, -1 * itemSlot.GetComponent<RectTransform>().rect.height * canvas.scaleFactor / 2, 0);
	}

	bool CheckIfItemIsInInventory(Item item) {
		for (int i = 0; i < items.Count; i++) {
			if (items [i].ID == item.ID) {
				return true;
			}
		}
		return false;
	}
}
