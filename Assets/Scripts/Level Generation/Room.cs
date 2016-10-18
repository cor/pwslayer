﻿using UnityEngine;
using System.Collections;


[System.Serializable]
public class Room {

	public Position position;
	public Size size;

	public Room(Position position, Size size) {

		this.position = position;
		this.size = size;
	}

}
