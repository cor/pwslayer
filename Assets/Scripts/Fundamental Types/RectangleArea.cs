using UnityEngine;
using System.Collections;


[System.Serializable]
public class RectangleArea {

	public Position position;
	public Size size;

	public RectangleArea(Position position, Size size) {

		this.position = position;
		this.size = size;
	}

}
