using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyDefinition {
	public Position position;

	public EnemyDefinition(Position position) {
		this.position = position;
	}
}
