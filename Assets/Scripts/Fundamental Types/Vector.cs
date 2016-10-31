using UnityEngine;
using System.Collections;

[System.Serializable]
public class Vector
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
}
