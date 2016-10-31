using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Position {
	public int x;
	public int y; 

	public Position(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public static Position operator +(Position p, Vector v) 
	{
		return new Position(p.x + v.dx, p.y + v.dy);
	}
}
