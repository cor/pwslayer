using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour {

	// Use this for initialization
	void OnDisable () {
		EventLogger eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();
		eventLogger.ToLog("Im Disabled af");
	}
	
	// Update is called once per frame
	void OnEnable() {
		EventLogger eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();
		eventLogger.ToLog("I aint not disabled wtf u talking about man");
	}
}
