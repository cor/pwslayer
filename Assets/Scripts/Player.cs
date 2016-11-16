using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player: MonoBehaviour {

	public Position position;
	public bool flipped;

	// Animation
	public float smoothTime = 0.3f;
	private Vector3 velocity = Vector3.zero;


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

	public void Move (Direction direction) {

		Level level = GameObject.FindWithTag("Level").GetComponent<Level>();

		if (level.CanMoveToTile(position + direction.ToVector())) {
			position += direction.ToVector ();
		}
	}
		
	void Update () {

		foreach(var keymap in directionKeymaps) {
			if (Input.GetKeyDown (keymap.Key)) {
				Move (keymap.Value);
				break;
			}
		}
		Render ();
	}

	public void Render() {

		// Update the GameObjects's position to represent the model's Position 
		Vector3 targetPosition = new Vector3 (position.x, position.y, transform.position.z);
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

		// Update scale to show flipped
		transform.localScale = new Vector3 (
			(flipped ? -1 : 1), 
			transform.localScale.y, 
			transform.localScale.z);
	}
}
