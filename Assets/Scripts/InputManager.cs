using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {

			// Get Player and mouse click
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			GameObject player = GameObject.FindWithTag ("Player");

			// Calculate Direction
			int dx = Mathf.RoundToInt(ray.origin.x - player.transform.position.x);
			int dy = Mathf.RoundToInt(ray.origin.y - player.transform.position.y);
			Direction direction = new Vector (dx, dy).ToDirection ();


			if (dx != 0 || dy != 0) {
				// Move Player in direction
				player.GetComponent<Player>().Move(direction);
			}
				
		}
	}
}
