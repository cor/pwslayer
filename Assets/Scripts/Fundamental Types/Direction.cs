using UnityEngine;
using System.Collections;

[System.Serializable]
public enum Direction
{
	North,
	NorthEast,
	East,
	SouthEast,
	South,
	SouthWest,
	West,
	NorthWest
}


public static class DirectionMethods
{

	public static Vector ToVector(this Direction direction)
	{
		int dx = 0;
		int dy = 0;

		switch (direction) {
		case Direction.North:
			dy = 1;
			break;
		case Direction.NorthEast:
			dy = 1;
			dx = 1;
			break;
		case Direction.East:
			dx = 1;
			break;
		case Direction.SouthEast:
			dx = 1;
			dy = -1;
			break;
		case Direction.South:
			dy = -1;
			break;
		case Direction.SouthWest:
			dy = -1;
			dx = -1;
			break;
		case Direction.West:
			dx = -1;
			break;
		case Direction.NorthWest:
			dx = -1;
			dy = 1;
			break;
		}

		return new Vector (dx, dy);
	}
}
