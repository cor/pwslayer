﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public Ray ray;
	public bool clickedOnEnemy;
	public Enemy enemy;

	public void CheckArrayOfEnemies(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		Player player = GameObject.FindWithTag ("Player").GetComponent<Player>();
		for (int i = 0; i <enemies.Length; i++){
			enemy = enemies[i].GetComponent<Enemy>();
			if (enemy.position.x == Mathf.RoundToInt(ray.origin.x)&&enemy.position.y == Mathf.RoundToInt(ray.origin.y)
			&& (player.position.x - enemy.position.x) <= 1 && (player.position.x - enemy.position.x) >= -1 
			&& (player.position.y - enemy.position.y) <= 1 && (player.position.y - enemy.position.y) >= -1){
				player.Combat();
				clickedOnEnemy = true;
			}
		}
	}
		
			

	private bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}

	private bool IsTouchOverUIObject(Touch touch) {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(touch.position.x, touch.position.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 1;
	}


	Dictionary<KeyCode, Direction> directionKeymaps = new Dictionary<KeyCode, Direction>() {
		{ KeyCode.Q, Direction.NorthWest },
		{ KeyCode.W, Direction.North },
		{ KeyCode.E, Direction.NorthEast },
		{ KeyCode.D, Direction.East },
		{ KeyCode.C, Direction.SouthEast },
		{ KeyCode.X, Direction.South },
		{ KeyCode.Z, Direction.SouthWest },
		{ KeyCode.A, Direction.West }
	};
		

	
	// Update is called once per frame
	void Update () {

		GameObject player = GameObject.FindWithTag ("Player");
		
		foreach(var keymap in directionKeymaps) {
			if (Input.GetKeyDown (keymap.Key)) {
				player.GetComponent<Player>().Move (keymap.Value);
				break;
			}
		}

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Ended && !IsTouchOverUIObject(touch)) {
			// if (touch.phase == TouchPhase.Ended) {

				ray = Camera.main.ScreenPointToRay (touch.position);
				CheckArrayOfEnemies();
				if(clickedOnEnemy){
					clickedOnEnemy = false;
				}
				else {		
					// Check if there is an item to be picked up
					int dx = Mathf.RoundToInt (ray.origin.x - player.transform.position.x);
					int dy = Mathf.RoundToInt (ray.origin.y - player.transform.position.y);
					
					Level level = GameObject.FindWithTag("Level").GetComponent<Level>();
					Position clickInLevelPosition = new Position(Mathf.RoundToInt(ray.origin.x), Mathf.RoundToInt(ray.origin.y));
					Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

					
					DroppedItem droppedItem = level.DroppedItemAtPosition(clickInLevelPosition);
					if (droppedItem != null) {
						inventory.AddItem(droppedItem.itemID);
						level.RemoveDroppedItem(clickInLevelPosition);
						
						EventLogger eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();
						eventLogger.ToLog("Picked up " + inventory.database.FetchItemByID(droppedItem.itemID).Title);

					} else if (level.ChestAtPosition(clickInLevelPosition) != -1) {
						ChestManager chestManager = GameObject.Find("ChestManager").GetComponent<ChestManager>();
						chestManager.SetOpen(true);
						
					} else {
						// If there isn't an item to be picked up, move the player in the cursor direcitiion
						Direction? direction = new Vector (dx, dy).ToDirection ();
						if (direction.HasValue) {
							player.GetComponent<Player> ().Move (direction.Value);
						}
						
					}
				}
			}
		}

		if (Input.GetMouseButtonDown (0)) {			
			// Read as: If the poiter is NOT on a UI element
			if (!IsPointerOverUIObject()) {
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				CheckArrayOfEnemies();
				if(clickedOnEnemy){
					clickedOnEnemy = false;
				}
				else {		
					// Check if there is an item to be picked up
					int dx = Mathf.RoundToInt (ray.origin.x - player.transform.position.x);
					int dy = Mathf.RoundToInt (ray.origin.y - player.transform.position.y);
					
					Level level = GameObject.FindWithTag("Level").GetComponent<Level>();
					Position clickInLevelPosition = new Position(Mathf.RoundToInt(ray.origin.x), Mathf.RoundToInt(ray.origin.y));
					Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();

					
					DroppedItem droppedItem = level.DroppedItemAtPosition(clickInLevelPosition);
					if (droppedItem != null) {
						inventory.AddItem(droppedItem.itemID);
						level.RemoveDroppedItem(clickInLevelPosition);
						
						EventLogger eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();
						eventLogger.ToLog("Picked up " + inventory.database.FetchItemByID(droppedItem.itemID).Title);

					} else if (level.ChestAtPosition(clickInLevelPosition) != -1) {
						ChestManager chestManager = GameObject.Find("ChestManager").GetComponent<ChestManager>();
						chestManager.SetOpen(true);
						
					} else {
						// If there isn't an item to be picked up, move the player in the cursor direcitiion
						Direction? direction = new Vector (dx, dy).ToDirection ();
						if (direction.HasValue) {
							player.GetComponent<Player> ().Move (direction.Value);
						}
						
					}
					
				}

			}

		}
	}
}
