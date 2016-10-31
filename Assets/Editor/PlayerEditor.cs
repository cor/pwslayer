using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Player))]
public class PlayerEditor : Editor {

	public override void OnInspectorGUI() {
		Player player = (Player) target;

		DrawDefaultInspector ();

		if(GUILayout.Button("Update")) {
			player.Render();
		}
	}
}
