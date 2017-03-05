using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

	public float HealthRemaining;
	void Update(){
		transform.localScale = new Vector3((float)HealthRemaining,1,1);
	}
}
