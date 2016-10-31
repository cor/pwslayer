using UnityEngine;
using System.Collections;

public class Player: MonoBehaviour {

	public Position position;
	public bool flipped;

	void Move(Direction direction) {

		Level level = GameObject.FindWithTag("Level").GetComponent<Level>();


		if (level.CanMoveToTile(position + direction.ToVector())) {
			position += direction.ToVector ();
		}
	}
		
	void Update () {


		if (Input.GetKeyDown(KeyCode.Q)) { Move (Direction.NorthWest); }
		else if (Input.GetKeyDown(KeyCode.W)) { Move (Direction.North); }
		else if (Input.GetKeyDown(KeyCode.E)) { Move (Direction.NorthEast); }
		else if (Input.GetKeyDown(KeyCode.D)) { Move (Direction.East); }
		else if (Input.GetKeyDown(KeyCode.C)) { Move (Direction.SouthEast); }
		else if (Input.GetKeyDown(KeyCode.X)) { Move (Direction.South); }
		else if (Input.GetKeyDown(KeyCode.Z)) { Move (Direction.SouthWest); }
		else if (Input.GetKeyDown(KeyCode.A)) { Move (Direction.West); }

		Render ();
	}

	public void Render() {

		// Update the GameObjects's position to represent the model's Position
		transform.position = new Vector3 (position.x, position.y, transform.position.z);

		// Update scale to show flipped
		transform.localScale = new Vector3 (
			(flipped ? -1 : 1), 
			transform.localScale.y, 
			transform.localScale.z);
	}
}
