﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLogger : MonoBehaviour {

	private GUIStyle guiStyle = new GUIStyle();
	private int fontSize = new int();
	public int maxLines = new int();
	private Queue<string> queue = new Queue<string>();
	private string eventLog = "";

	
	public void ToLog(string activity) {
			if (queue.Count >= maxLines){
				queue.Dequeue();
				queue.Enqueue(activity);
				eventLog = "";
			}
			else{
				queue.Enqueue(activity);
				eventLog = "";
			}
			foreach (string i in queue){
					eventLog = eventLog + i + "\n";
			}
		
        }
		
	void OnGUI () {
		guiStyle.fontSize = Mathf.RoundToInt(Screen.height*0.021f);
		guiStyle.normal.textColor = Color.white;	
		GUI.Label(
			// x, y, w, h
			new Rect(8, 40, 0.6f*Screen.width, 0.2f*Screen.height), 
		 
		 eventLog, 
		 guiStyle);
	}
}
