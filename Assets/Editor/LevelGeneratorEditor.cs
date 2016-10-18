using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (LevelGenerator))]
public class LevelGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		LevelGenerator levelGenerator = (LevelGenerator) target;

		if (DrawDefaultInspector()) {
			if(levelGenerator.shouldAutoUpdate) {
				levelGenerator.GenerateLevel();
			}
		}

		if(GUILayout.Button("Generate")) {
			levelGenerator.GenerateLevel();
		}
	}
}
