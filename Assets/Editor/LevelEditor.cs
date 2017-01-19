using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Level))]
public class LevelEditor : Editor {

	public override void OnInspectorGUI() {
		Level level = (Level) target;

		if (DrawDefaultInspector()) {
			if(level.shouldAutoUpdate && level.customGenerationEnabled) {
				level.Generate();
			}
		}

		if (GUILayout.Button("Generate")) {
			level.Generate();
		}
	}
}
