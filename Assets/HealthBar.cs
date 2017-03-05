using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public float HealthRemaining;
	void Update(){
		transform.localScale = new Vector3(HealthRemaining,1f,1f);
	}
}
