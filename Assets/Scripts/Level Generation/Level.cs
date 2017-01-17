using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {


	public bool customGenerationEnabled = true;
	
	public Size size;


	public List<Room> rooms = new List<Room>();
	public List<Tunnel> tunnels = new List<Tunnel>();
	public List<Opening> openings = new List<Opening>();
	public List<EnemyDefinition> enemyDefinitions = new List<EnemyDefinition>();

	public GameObject voidTile;
	public GameObject groundTile;
	public GameObject wallTile;
	public GameObject doorTile;
	public GameObject slime;

	string[,] tiles;
	List<GameObject> enemies = new List<GameObject>();


	// Constraints
	public Size minimumRoomSize = new Size(4, 4);
	public Size maximumRoomSize = new Size(10, 10);

	public int minimumTunnelLength = 5;
	public int maximumTunnelLength = 20;
	
	public bool shouldAutoUpdate;

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
			ClearModel();
			GenerateRandomModel();
		}
		
		Render ();

		if (customGenerationEnabled) {
			AddEnemiesFromEnemyDefinitions ();
		}

	}
		
	void AddEnemiesFromEnemyDefinitions() {
		for (int i = 0; i < enemyDefinitions.Count; i++) {

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
	
	void ClearModel() {
		DeleteRooms();
		DeleteOpenings();
		DeleteTunnels();
		DeleteEnemyDefinitions();
	}
	void DeleteRooms() {
		rooms = new List<Room>();
	}
	
	void DeleteTunnels() {
		tunnels = new List<Tunnel>();
	}
	
	void DeleteOpenings() {
		openings = new List<Opening>();
	}

	void DeleteEnemyDefinitions() {
		enemyDefinitions = new List<EnemyDefinition>();
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
		for (int i = 0; i < rooms.Count; i++) {
			AddRoomToTiles (rooms[i]);
		}
	}

	void GenerateRandomModel() {

		Size firstRoomSize = new Size(Random.Range(minimumRoomSize.width, maximumRoomSize.width), Random.Range(minimumRoomSize.height, maximumRoomSize.height));
		Position firstRoomPosition = new Position((size.width / 2) - (firstRoomSize.width / 2), (size.height / 2) - (firstRoomSize.height / 2));
		Room firstRoom = new Room(firstRoomPosition, firstRoomSize);

		rooms.Add(firstRoom);
		AddRoomToTiles(firstRoom);
		
		AddTunnel();
		foreach (Tunnel tunnel in tunnels)
		{
			AddTunnelToTiles(tunnel);
		}
		

	}

	void AddTunnel() {
		Room randomRoom = rooms[Random.Range(0, rooms.Count - 1)];
		
		List<Opening> wallOpenings = new List<Opening>();


		// Get all horizontal wall tiles (excluding corners by going from 1 to width -1)
		for (int i = 1; i < randomRoom.size.width - 1; i++) {
			Position southWallTilePosition = new Position(randomRoom.position.x + i, randomRoom.position.y);
			Position northWallTilePosition = new Position(randomRoom.position.x + i, randomRoom.position.y + randomRoom.size.height - 1);
			
			wallOpenings.Add(new Opening(northWallTilePosition, Direction.North));
			wallOpenings.Add(new Opening(southWallTilePosition, Direction.South));
		}

		for (int i = 1; i < randomRoom.size.height - 1; i++) {
			Position westWallTilePosition = new Position(randomRoom.position.x, randomRoom.position.y + i);
			Position eastWallTilePosition = new Position(randomRoom.position.x + randomRoom.size.width - 1, randomRoom.position.y + i);
			
			wallOpenings.Add(new Opening(eastWallTilePosition, Direction.East));
			wallOpenings.Add(new Opening(westWallTilePosition, Direction.West));
		}

		Opening randomWallOpening = wallOpenings[Random.Range(0, wallOpenings.Count - 1)];

		tiles[randomWallOpening.position.x, randomWallOpening.position.y] = "ground";

		int tunnelLength = Random.Range(minimumTunnelLength, maximumTunnelLength);

		Size tunnelSize;
		Vector tunnelRectPositionDelta;

		switch (randomWallOpening.direction)
		{
			case Direction.North:
			tunnelSize = new Size(3, tunnelLength);
			tunnelRectPositionDelta = new Vector(-1, +1);
			break;
			
			case Direction.East:
			tunnelSize = new Size(tunnelLength, 3);
			tunnelRectPositionDelta = new Vector(+1, -1);
			break;
			
			case Direction.South:
			tunnelSize = new Size(3, tunnelLength);
			tunnelRectPositionDelta = new Vector(-1, -(tunnelSize.height));
			break;
			
			case Direction.West:	
			tunnelSize = new Size(tunnelLength, 3);
			tunnelRectPositionDelta = new Vector(-(tunnelSize.width), -1);
			break;
			
			default:
			Debug.LogError("Tunnel's can't be made in diagonal directions");
			tunnelSize = new Size(0,0);
			tunnelRectPositionDelta = new Vector(0,0);
			break;
		}

		if (RectangleAreaIsEmpty(new RectangleArea(randomWallOpening.position + tunnelRectPositionDelta, tunnelSize))) {
			tunnels.Add(new Tunnel(randomWallOpening.position + randomWallOpening.direction.ToVector(), tunnelLength, randomWallOpening.direction));
		}
		
	}
	void AddRoomToTiles(Room room) {
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

	void AddTunnelToTiles(Tunnel tunnel) {
		for (int i = 0; i < tunnel.length; i++) {
			{
				switch (tunnel.direction)
				{
					case Direction.North:
					//path
					tiles[tunnel.position.x, tunnel.position.y + i] = "ground";

					//walls
					tiles[tunnel.position.x + 1, tunnel.position.y + i] = "wall";
					tiles[tunnel.position.x - 1, tunnel.position.y + i] = "wall";
					break;
					
					case Direction.East:
					//path
					tiles[tunnel.position.x + i, tunnel.position.y] = "ground";

					//walls
					tiles[tunnel.position.x + i, tunnel.position.y + 1] = "wall";
					tiles[tunnel.position.x + i, tunnel.position.y - 1] = "wall";
					break;
					
					case Direction.South:
					//path
					tiles[tunnel.position.x, tunnel.position.y - i] = "ground";

					//walls
					tiles[tunnel.position.x + 1, tunnel.position.y - i] = "wall";
					tiles[tunnel.position.x - 1, tunnel.position.y - i] = "wall";
					break;
					
					case Direction.West:
					//path
					tiles[tunnel.position.x - i, tunnel.position.y] = "ground";

					//walls
					tiles[tunnel.position.x - i, tunnel.position.y + 1] = "wall";
					tiles[tunnel.position.x - i, tunnel.position.y - 1] = "wall";
					break;

					default:
					Debug.LogError("Tunnel's can't be made in diagonal directions");
					break;
				}
				
			}
			
		}
		
	}

	bool RectangleAreaIsEmpty(RectangleArea rectangleArea) {
		for (int x = 0; x < rectangleArea.size.width; x++) {
			for (int y = 0; y < rectangleArea.size.height; y++) {

				// If position is out of bounds, then the area isn't Empty
				if (!PositionIsInLevel(new Position(rectangleArea.position.x + x, rectangleArea.position.y + y))) {
					return false;
				}

				// If one tile is not void, then the area isn't empty
				if (!(tiles[rectangleArea.position.x + x, rectangleArea.position.y + y] == "void")) {
					return false;
				}
			}
		}
		return true;
	}

	bool PositionIsInLevel(Position position) {
		return !(position.x < tiles.GetLowerBound(0) ||
		    	 position.x > tiles.GetUpperBound(0) ||
				 position.y < tiles.GetLowerBound(1) ||
 				 position.y > tiles.GetUpperBound(1));
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
