using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	public Size levelSize;

	public Room room;
	public Position roomPosition;

	public GameObject voidTile;
	public GameObject groundTile;
	public GameObject wallTile;
	public GameObject doorTile;

	string[,] level;

	// Use this for initialization
	void Start () {
		GenerateLevelModel ();
		RenderLevel ();
	}

	void SpawnTile(GameObject tile, Position position) {
		
		GameObject groundClone = (GameObject) Instantiate (tile, new Vector3 (position.x, position.y, 0), transform.rotation);
	}

	void GenerateLevelModel() {
		level = new string[levelSize.width, levelSize.height];

		for (int x = 0; x < level.GetLength(0); x++) {
			for (int y = 0; y < level.GetLength(1); y++) {
				level [x, y] = "void";
			}
		}
			
		AddRoomToModel (room, roomPosition);
	}

	void AddRoomToModel(Room roomToAdd, Position position) {
		// Place ground everywhere
		for (int x = 0; x < room.size.width; x++) {
			for (int y = 0; y < room.size.height; y++) {
				level [position.x + x, position.y + y] = "ground";
			}
		}

		//Replace the outer rows with wall
		for (int x = 0; x < room.size.width; x++) {
			level [position.x + x, position.y + 0] = "wall";
			level [position.x + x, position.y + (room.size.height - 1)] = "wall";
		}

		for (int y = 0; y < room.size.height; y++) {
			level [position.x + 0, position.y + y] = "wall";
			level [position.x + (room.size.width - 1), position.y + y] = "wall"; 
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
