using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Item item;
	public int amount;
	public int slot;

	private Inventory inventory;
	private Vector2 offset;


	void Start()
	{
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
		if (item != null) {
			offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
			
			this.transform.SetParent(this.transform.parent.parent.parent);
			this.transform.position = eventData.position - offset;	
			GetComponent<CanvasGroup>().blocksRaycasts = false;

		}
    }

    public void OnDrag(PointerEventData eventData)
    {
		if (item != null) {
			this.transform.position = eventData.position - offset;	
		}
    }

    public void OnEndDrag(PointerEventData eventData) {

		Ray ray = Camera.main.ScreenPointToRay (eventData.position);
		Position pointerInLevelPosition = new Position(Mathf.RoundToInt(ray.origin.x), Mathf.RoundToInt(ray.origin.y));

		// If the Item is not dropped on the inventory but on the ground
		if (!EventSystem.current.IsPointerOverGameObject()) {
			
			// Add it to the level
			Level level = GameObject.FindWithTag("Level").GetComponent<Level>();
			if (level.DroppedItemAtPosition(pointerInLevelPosition) == null) {
				
				// There is room on the ground for an item, drop it.
				level.AddDroppedItem(item.ID, pointerInLevelPosition);
				
				this.transform.SetParent(inventory.slots[slot].transform);
				inventory.RemoveItem(slot);
			}
			
		}

		this.transform.SetParent(inventory.slots[slot].transform);
		SnapPositionToCenterOfParent();

		GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

	public void SnapPositionToCenterOfParent() {
		RectTransform rectTransform = this.GetComponent<RectTransform>();
		rectTransform.offsetMin = new Vector2(0, 0);
		rectTransform.offsetMax = new Vector2(0, 0);
	}
}
