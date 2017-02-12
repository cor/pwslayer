using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Tunnel {

	public Position position;
	public int length;
	public Direction direction;

	public Tunnel(Position position, int length, Direction direction) {

		this.position = position;
		this.length = length;
		this.direction = direction;
	}

	Size Size() {

		switch (direction)
		{
			case Direction.North:
			return new Size(3, length);
			
			case Direction.East:
			return new Size(length, 3);
			
			case Direction.South:
			return new Size(3, length);
			
			case Direction.West:	
			return new Size(length, 3);
			
			default:
			Debug.LogError("Tunnel's can't be made in diagonal directions");
			return new Size(0,0);
		}

	}

}
