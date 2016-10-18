using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public float cameraMoveSpeedMultiplier;

	// Update is called once per frame
	void Update () {
		float x = transform.position.x + (Input.GetAxis ("Horizontal") * cameraMoveSpeedMultiplier);
		float y = transform.position.y + (Input.GetAxis ("Vertical") * cameraMoveSpeedMultiplier);
		transform.position = new Vector3 (x, y, transform.position.z);
	}
}
