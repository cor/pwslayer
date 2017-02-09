using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Opening {
	public Position position;
	public Direction direction;

	public string connectedTo;

	public Opening(Position position, Direction direction, string connectedTo) {
		this.position = position;
		this.direction = direction;
		this.connectedTo = connectedTo; // ex: "room" or "tunnel"
	}

}

public struct UsedOpening {
	public Opening opening;
	public bool rightIsStraight;
	public bool leftIsStraight;

	public UsedOpening(Opening opening, bool rightIsStraight, bool leftIsStraight) {
		this.opening = opening;
		this.rightIsStraight = rightIsStraight;
		this.leftIsStraight = leftIsStraight;
	}
}