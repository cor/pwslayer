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

	public static Direction Inverted(this Direction direction) {
		switch (direction)
		{
		case Direction.North:
		return Direction.South;
		break;

		case Direction.NorthEast:
		return Direction.SouthWest;
		break;
		
		case Direction.East:
		return Direction.West;
		break;
		
		case Direction.SouthEast:
		return Direction.NorthWest;
		break;
		
		case Direction.South:
		return Direction.North;
		break;
		
		case Direction.SouthWest:
		return Direction.NorthEast;
		break;
			
		case Direction.West:
		return Direction.East;
		break;
		
		case Direction.NorthWest:
		return Direction.SouthEast;
		break;

		default:
		return Direction.North;
		break;
		}
	}
}