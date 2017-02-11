using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedItemHighlightController : MonoBehaviour {

	public Canvas canvas;
	private Inventory inventory;

	// move to correct slot animation
	public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
	}
	
	// Update is called once per frame
	void Update () {
		GameObject itemSlot = inventory.slots[inventory.equipedItemSlotID];
		
		// Calculate the correct position
		Vector3 targetPosition = 
			itemSlot.transform.position + 
			new Vector3(
				itemSlot.GetComponent<RectTransform>().rect.width * canvas.scaleFactor / 2,
				-1 * itemSlot.GetComponent<RectTransform>().rect.height * canvas.scaleFactor / 2, 
				0);
				
		// Smoothly go to the calculated position
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		
	}
}
