using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour {

	// Pinch to zoom
	public float zoomSpeed = .5f;

	public float dampTime;
	private Vector3 velocity = Vector3.zero;
	public Transform target;



	void Update() {
	
		if (Input.touchCount == 2) {
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			float deltaMagnitudediff = prevTouchDeltaMagnitude - touchDeltaMag;

			Camera cam = GetComponent<Camera> ();
			cam.orthographicSize += deltaMagnitudediff * zoomSpeed;
			cam.orthographicSize = Mathf.Max (cam.orthographicSize, .1f);

		}
	}


	// Update is called once per frame
	void LateUpdate () 
	{

		if (target) {
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); 
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}

	}



}
