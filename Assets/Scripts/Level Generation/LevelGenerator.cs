using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	public Size levelSize;
	public GameObject levelParent;

	public Room[] rooms;

	public GameObject voidTile;
	public GameObject groundTile;
	public GameObject wallTile;
	public GameObject doorTile;

	public bool shouldAutoUpdate;

	string[,] level;

	// Use this for initialization
	void Start () {
		GenerateLevel ();
	}

	public void GenerateLevel() {

		DeleteLevel ();
		GenerateLevelModel ();
		RenderLevel ();
	}

	void DeleteLevel() {
		while(levelParent.transform.childCount != 0){
			DestroyImmediate(levelParent.transform.GetChild(0).gameObject);
		}
	}

	bool CanMoveToTile(Position position) {
		return level [position.x, position.y] == "ground";
	}

	void SpawnTile(GameObject tile, Position position) {
		
		GameObject tileClone = (GameObject) Instantiate (tile, new Vector3 (position.x, position.y, 0), transform.rotation);
		tileClone.transform.parent = levelParent.transform;
	}

	void GenerateLevelModel() {
		level = new string[levelSize.width, levelSize.height];

		for (int x = 0; x < level.GetLength(0); x++) {
			for (int y = 0; y < level.GetLength(1); y++) {
				level [x, y] = "void";
			}
		}

		for (int i = 0; i < rooms.GetLength(0); i++) {
			AddRoomToModel (rooms[i]);	
		}

	}

	void AddRoomToModel(Room room) {
		// Place ground everywhere
		for (int x = 0; x < room.size.width; x++) {
			for (int y = 0; y < room.size.height; y++) {
				level [room.position.x + x, room.position.y + y] = "ground";
			}
		}

		//Replace the outer rows with wall
		for (int x = 0; x < room.size.width; x++) {
			level [room.position.x + x, room.position.y + 0] = "wall";
			level [room.position.x + x, room.position.y + (room.size.height - 1)] = "wall";
		}

		for (int y = 0; y < room.size.height; y++) {
			level [room.position.x + 0, room.position.y + y] = "wall";
			level [room.position.x + (room.size.width - 1), room.position.y + y] = "wall"; 
		}
	}



	void RenderLevel() {
		for (int x = 0; x < level.GetLength(0); x++) {
			for (int y = 0; y < level.GetLength(1); y++) {

				string tileName = level [x, y];
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
		
	
	// Update is called once per frame
	void Update () {
	
	}
}
