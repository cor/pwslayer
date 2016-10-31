using UnityEngine;
using System.Collections;

public class Player: MonoBehaviour {

	public Position position;

	void Move(Direction direction) {

		Level level = GameObject.FindWithTag("Level").GetComponent<Level>();


		if (level.CanMoveToTile(position + direction.ToVector())) {
			position += direction.ToVector ();
		}
	}
		
	void Update () {


		if (Input.GetKeyDown(KeyCode.D)) {
			Move (Direction.East);	
		} else if (Input.GetKeyDown(KeyCode.A)) {
			Move (Direction.West);
		} else if (Input.GetKeyDown (KeyCode.W)) {
			Move (Direction.North);
		} else if (Input.GetKeyDown (KeyCode.S)) {
			Move (Direction.South);
		}

		UpdatePosition ();
	}

	public void UpdatePosition() {
		transform.position = new Vector3 (position.x, position.y, transform.position.z);
	}
}
