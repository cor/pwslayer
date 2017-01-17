using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Opening {
	public Position position;
	public Direction direction;

	public Opening(Position position, Direction direction) {
		this.position = position;
		this.direction = direction;
	}

}