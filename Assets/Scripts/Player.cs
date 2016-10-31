using UnityEngine;
using System.Collections;

public class Player {

	public Position position;

	void Move(Direction direction) {
		position += direction.ToVector ();
	}
}
