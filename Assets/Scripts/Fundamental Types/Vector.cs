using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Vector
{
	public int dx;
	public int dy;

	public Vector(int dx, int dy) {
		this.dx = dx;
		this.dy = dy;
	}
		
	public static Vector operator +(Vector v1, Vector v2) 
	{
		return new Vector(v1.dx + v2.dx, v1.dy + v2.dy);
	}

	public Direction ToDirection() {
		if (dx == 0 && dy > 0)
			return Direction.North;
		else if (dx > 0 && dy > 0)
			return Direction.NorthEast;
		else if (dx > 0 && dy == 0)
			return Direction.East;
		else if (dx > 0 && dy < 0)
			return Direction.SouthEast;
		else if (dx == 0 && dy < 0)
			return Direction.South;
		else if (dx < 0 && dy < 0)
			return Direction.SouthWest;
		else if (dx < 0 && dy == 0)
			return Direction.West;
		else
			return Direction.NorthWest;
	}
}
