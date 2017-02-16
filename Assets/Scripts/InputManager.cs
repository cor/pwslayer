using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {

	public int j;
	public Ray ray;
	public bool clickedOnEnemy;

void CheckArrayOfEnemies(){
	GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
	Player player = GameObject.FindWithTag ("Player").GetComponent<Player>();
	for (int i = 0; i <enemies.Length; i++){
		if (enemies[i].GetComponent<Enemy>().position.x == Mathf.RoundToInt(ray.origin.x)&&enemies[i].GetComponent<Enemy>().position.y == Mathf.RoundToInt(ray.origin.y)){
			j=i;
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

		if (Input.GetMouseButtonDown (0)) {			

			// Read as: If the poiter is NOT on a UI element
			if (!IsPointerOverUIObject()) {
				
				CheckArrayOfEnemies();
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if(clickedOnEnemy){
					clickedOnEnemy = false;
				}
				else{	// Calculate Direction	
					int dx = Mathf.RoundToInt (ray.origin.x - player.transform.position.x);
					int dy = Mathf.RoundToInt (ray.origin.y - player.transform.position.y);
					Direction? direction = new Vector (dx, dy).ToDirection ();

					if (direction.HasValue) {
						player.GetComponent<Player> ().Move (direction.Value);
					}
					
				}

			}

		}
	}
}
