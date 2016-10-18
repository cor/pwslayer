using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Update is called once per frame
	void Update () {

		int dx = 0;
		int dy = 0;

		if (Input.GetKeyDown(KeyCode.D)) {
			dx = 1;
		} else if (Input.GetKeyDown(KeyCode.A)) {
			dx = -1;
		} else if (Input.GetKeyDown (KeyCode.W)) {
			dy = 1;
		} else if (Input.GetKeyDown (KeyCode.S)) {
			dy = -1;
		}

		transform.position = new Vector3 (transform.position.x + dx, transform.position.y + dy, transform.position.z);
	}
}
