using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour {
	public int itemID;
	public Position position;
	Inventory inventory;
	
	public void Init (int id, Position position) {
		itemID = id;
		this.position = position;
		
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		Item itemToDisplay = inventory.database.FetchItemByID(itemID);
		gameObject.GetComponent<SpriteRenderer>().sprite = itemToDisplay.Sprite;
	}

	void Update()
	{
		transform.position = new Vector3(position.x, position.y, -1);
	}
}
