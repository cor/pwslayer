using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Position position;

	void Move(Direction direction) {
		position += direction.ToVector ();
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

		transform.position = new Vector3 (position.x, position.y, transform.position.z);
	}
}
