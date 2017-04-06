using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;


public class EnemyDefinitionDatabase : MonoBehaviour {

	private List<EnemyDefinition> database = new List<EnemyDefinition>();
	private JsonData enemyData;
	
	void Start () {
		enemyData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Enemies.json"));
		ConstructEnemyDefinitionDatabase();

	}
	
	public EnemyDefinition FetchEnemyDefinitionByID(int id) {

		for (int i = 0; i < database.Count; i++)
		{
			if (database[i].id == id) {
				return database[i];
			}
		}

		return null;
	}

	void ConstructEnemyDefinitionDatabase() {
		for (int i = 0; i < enemyData.Count; i++) {
			
			database.Add(new EnemyDefinition(
				(int)enemyData[i]["id"],
				enemyData[i]["name"].ToString(),
				enemyData[i]["slug"].ToString(),
				(int)enemyData[i]["stats"]["attack"],
				(int)enemyData[i]["stats"]["health"],
				(int)enemyData[i]["stats"]["health"],
				(int)enemyData[i]["stats"]["armor"]
			));
		}
	}
}
