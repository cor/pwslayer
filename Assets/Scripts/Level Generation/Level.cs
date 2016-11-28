using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public Size size;

	public Room[] rooms;

	public GameObject voidTile;
	public GameObject groundTile;
	public GameObject wallTile;
	public GameObject doorTile;

	public bool shouldAutoUpdate;

	string[,] tiles;
	public GameObject[] enemies;

	// Use this for initialization
	void Start () {
		Generate ();
		enemies = GameObject.FindGameObjectsWithTag ("Enemy");
	}

	public void Generate() {

		DeleteTiles ();
		GenerateModel ();
		Render ();
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

	void GenerateModel() {
		tiles = new string[size.width, size.height];

		for (int x = 0; x < tiles.GetLength(0); x++) {
			for (int y = 0; y < tiles.GetLength(1); y++) {
				tiles [x, y] = "void";
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
}
