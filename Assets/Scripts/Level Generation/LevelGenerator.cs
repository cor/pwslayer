using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	public Room room;

	public GameObject ground;
	public GameObject wall;
	public GameObject door;

	string[,] level;


	// Use this for initialization
	void Start () {
		GenerateLevelModel ();
		RenderLevel ();
	}

	void SpawnTile(GameObject tile, Vector2 position) {
		
		GameObject groundClone = (GameObject) Instantiate (tile, new Vector3 (position.x, position.y, 0), transform.rotation);
	}

	void GenerateLevelModel() {

		level = new string[room.width, room.height];

		// Place ground everywhere
		for (int x = 0; x < room.width; x++) {
			for (int y = 0; y < room.height; y++) {
				level [x, y] = "ground";
			}
		}

		//Replace the outer rows with wall
		for (int x = 0; x < room.width; x++) {
			level [x, 0] = "wall";
			level [x, room.height - 1] = "wall";
		}

		for (int y = 0; y < room.height; y++) {
			level [0, y] = "wall";
			level [room.width - 1, y] = "wall"; 
		}
	}

	void RenderLevel() {
		for (int x = 0; x < level.GetLength(0); x++) {
			for (int y = 0; y < level.GetLength(1); y++) {

				string tileName = level [x, y];
				GameObject tile = TileForName(tileName);

				SpawnTile(tile, new Vector2(x, y));
			}
		}
	}

	GameObject TileForName(string name) {
		
		switch (name) {
		case "ground":
			return ground;
		case "wall":
			return wall;
		case "door":
			return door;
		default:
			return null;
		}

	}
		
	
	// Update is called once per frame
	void Update () {
	
	}
}
