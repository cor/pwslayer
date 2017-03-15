using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Chest {
	public Position position;
	public int[] items;

	// items should be of size 16
	public Chest(Position position, int[] items) {
		this.position = position;
		this.items = items;
	}

}
