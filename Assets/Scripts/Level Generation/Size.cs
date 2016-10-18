using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Size {
	public int width;
	public int height; 

	public Size(int width, int height) {
		this.width = width;
		this.height = height;
	}
}
