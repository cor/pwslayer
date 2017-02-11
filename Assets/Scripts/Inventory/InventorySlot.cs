using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler {


	public int id;
	private Inventory inventory;


	void Start() {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
	}

    public void OnDrop(PointerEventData eventData)
    {
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
		
		// If the equiped item is moved, make sure to update the EquipedItemSlotID.
		bool equipedItemIsMoved = droppedItem.slot == inventory.equipedItemSlotID;
			

		if (inventory.items[id].ID == -1) {
			
			inventory.items[droppedItem.slot] = new Item();
			inventory.items[id] = droppedItem.item;
			droppedItem.transform.SetParent(this.transform);

			droppedItem.slot = id;
		} else {
			Transform item = this.transform.GetChild(0);
			item.GetComponent<ItemData>().slot = droppedItem.slot;
			item.transform.SetParent(inventory.slots[droppedItem.slot].transform);
			item.GetComponent<ItemData>().SnapPositionToCenterOfParent();
			
			droppedItem.slot = id;
			droppedItem.transform.SetParent(this.transform);
			droppedItem.GetComponent<ItemData>().SnapPositionToCenterOfParent();

			
			inventory.items[droppedItem.slot] = item.GetComponent<ItemData>().item;
			inventory.items[id] = droppedItem.item;
		}

		if (equipedItemIsMoved) {
			inventory.EquipItem(id);
		}
		
    }
    public void OnPointerClick(PointerEventData eventData) {
        inventory.EquipItem(id);
		GetComponent<AudioSource>().Play();
    }
}
