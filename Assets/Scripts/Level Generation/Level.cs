using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {


	public Size size;

	public bool customGenerationEnabled = true;

	public Room[] rooms;
	public EnemyDefinition[] enemyDefinitions;

	public GameObject voidTile;
	public GameObject groundTile;
	public GameObject wallTile;
	public GameObject doorTile;
	public GameObject slime;

	public bool shouldAutoUpdate;
	string[,] tiles;
	List<GameObject> enemies = new List<GameObject>();


	public Size minimumRoomSize = new Size(4, 4);
	public Size maximumRoomSize = new Size(10, 10);


	// Use this for initialization
	void Start () {
		Generate ();
	}

	public void Generate() {

		DeleteTiles ();
		
		CreateEmptyModel();

		if (customGenerationEnabled) {
			GenerateCustomModel ();
		} else {
			GenerateRandomModel ();
		}
		
		Render ();

		if (customGenerationEnabled) {
			AddEnemiesFromEnemyDefinitions ();
		}

	}
		
	void AddEnemiesFromEnemyDefinitions() {
		for (int i = 0; i < enemyDefinitions.Length; i++) {

			Debug.Log ("Add enemy");

			Position enemyPosition = enemyDefinitions [i].position;

			GameObject enemyClone = (GameObject)Instantiate (slime, new Vector3 (enemyPosition.x, enemyPosition.y, -1), transform.rotation);
			enemyClone.transform.parent = transform;
			slime.GetComponent<Enemy> ().position = enemyPosition;

			enemies.Add(enemyClone)	;
			
		}
	}

	void DeleteTiles() {
		while(transform.childCount != 0){
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}


	void SpawnTile(GameObject tile, Position position) {
		
		GameObject tileClone = (GameObject) Instantiate (tile, new Vector3 (position.x, position.y, 0), transform.rotation);
		tileClone.transform.parent = transform;
	}




	void CreateEmptyModel() {
		tiles = new string[size.width, size.height];

		for (int x = 0; x < tiles.GetLength(0); x++) {
			for (int y = 0; y < tiles.GetLength(1); y++) {
				tiles [x, y] = "void";
			}
		}
	}

	void GenerateCustomModel() {
		for (int i = 0; i < rooms.GetLength(0); i++) {
			AddRoomToModel (rooms[i]);	
		}
	}

	void GenerateRandomModel() {

		Size firstRoomSize = new Size(Random.Range(minimumRoomSize.width, maximumRoomSize.width), Random.Range(minimumRoomSize.height, maximumRoomSize.height));
		Position firstRoomPosition = new Position((size.width / 2) - (firstRoomSize.width / 2), (size.height / 2) - (firstRoomSize.height / 2));
		Room firstRoom = new Room(firstRoomPosition, firstRoomSize);
		AddRoomToModel(firstRoom);

	}

	void AddRoomToModel(Room room) {
		// Place ground everywhere
		for (int x = 0; x < room.size.width; x++) {
			for (int y = 0; y < room.size.height; y++) {
				tiles [room.position.x + x, room.position.y + y] = "ground";
			}
		}

		//Replace the outer rows with wall
		for (int x = 0; x < room.size.width; x++) {
			tiles [room.position.x + x, room.position.y + 0] = "wall";
			tiles [room.position.x + x, room.position.y + (room.size.height - 1)] = "wall";
		}

		for (int y = 0; y < room.size.height; y++) {
			tiles [room.position.x + 0, room.position.y + y] = "wall";
			tiles [room.position.x + (room.size.width - 1), room.position.y + y] = "wall"; 
		}
	}



	void Render() {
		RenderTiles ();
	}

	void RenderTiles() {
		for (int x = 0; x < tiles.GetLength(0); x++) {
			for (int y = 0; y < tiles.GetLength(1); y++) {

				string tileName = tiles [x, y];
				GameObject tile = TileForName(tileName);

				SpawnTile(tile, new Position(x, y));
			}
		}
	}
		

	GameObject TileForName(string name) {
		
		switch (name) {
		case "ground":
			return groundTile;
		case "wall":
			return wallTile;
		case "door":
			return doorTile;
		case "void":
			return voidTile;
		default:
			return null;
		}

	}
		
	public bool CanMoveToTile(Position position) {
		return tiles [position.x, position.y] == "ground";
	}

	public GameObject EnemyIsOnTile(Position position) {
		for (int i = 0; i < enemies.Count; i++) {
			if (enemies [i].GetComponent<Enemy> ().position.x == position.x &&
			    enemies [i].GetComponent<Enemy> ().position.y == position.y) {
				return enemies[i];
			}
		}
		return null;
	}
}
