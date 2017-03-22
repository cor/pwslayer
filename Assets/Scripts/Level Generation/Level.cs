using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour {


	public bool customGenerationEnabled = true;
	
	public Size size;


	public int generationCycles;
	public int enemyCount;


	public List<Room> rooms = new List<Room>();
	public List<Tunnel> tunnels = new List<Tunnel>();
	public List<Chest> chests = new List<Chest>();
	
	public List<Opening> openingPossibilities =  new List<Opening>();
	public List<Opening> usedOpenings = new List<Opening>();
	public List<EnemyDefinition> enemyDefinitions = new List<EnemyDefinition>();


	public GameObject groundTile;
	public GameObject slime;
	public GameObject droppedItem;


	string[,] tiles;
	List<GameObject> enemies = new List<GameObject>();
	public List<GameObject> droppedItems = new List<GameObject>();


	// Constraints
	public Size minimumRoomSize = new Size(4, 4);
	public Size maximumRoomSize = new Size(10, 10);

	public int minimumTunnelLength = 5;
	public int maximumTunnelLength = 20;
	
	public bool shouldAutoUpdate;

	// Use this for initialization
	void Start () {
		Generate ();
		AddDroppedItem(3, new Position(51, 50));
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
		
		AddEnemiesFromEnemyDefinitions ();

	}
		
	void AddEnemiesFromEnemyDefinitions() {
		for (int i = 0; i < enemyDefinitions.Count; i++) {

			Position enemyPosition = enemyDefinitions [i].position;

			GameObject enemyClone = (GameObject)Instantiate (slime, new Vector3 (enemyPosition.x, enemyPosition.y, -1), transform.rotation);
			enemyClone.transform.parent = transform;
			slime.GetComponent<Enemy> ().position = enemyPosition;

			enemies.Add(enemyClone);
			
		}
	}


	void DeleteTiles() {
		while(transform.childCount != 0){
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}
	
	void ClearModel() {
		DeleteRooms();
		DeleteOpeningPossibilities();
		DelteUsedOpenings();
		DeleteTunnels();
		DeleteChests();
		DeleteEnemyDefinitions();
	}
	void DeleteRooms() {
		rooms = new List<Room>();
	}
	
	void DeleteTunnels() {
		tunnels = new List<Tunnel>();
	}

	void DeleteChests() {
		chests = new List<Chest>();
	}

	void DeleteOpeningPossibilities() {
		openingPossibilities = new List<Opening>();
	}
	
	void DelteUsedOpenings() {
		usedOpenings = new List<Opening>();
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
		foreach (Room room in rooms) {
			AddRoomToTiles(room);
		}
		
		for (int i = 0; i < generationCycles; i++) {
			RefreshOpeningPossibilities();
			AddTunnel();
			UpdateTiles();
			RefreshOpeningPossibilities();
			AddRoom();
			UpdateTiles();
		}
		
		for (int i = 0; i < generationCycles; i++) {
			AddChest();
			UpdateTiles();
		}

		for (int i = 0; i < enemyCount; i++) {
			AddEnemyDefinition();
		}

	}

	void UpdateTiles() {
			// So this is what life without .map() is like
			foreach (Tunnel tunnel in tunnels) { AddTunnelToTiles(tunnel); }
			foreach (Room room in rooms) { AddRoomToTiles(room); }
			foreach (Opening usedOpening in usedOpenings) { AddUsedOpeningToTiles(usedOpening); }
			foreach (Chest chest in chests) { AddChestToTiles(chest); }
	}

	void RefreshOpeningPossibilities() {

		DeleteOpeningPossibilities();
		
		foreach (Room room in rooms) {
			
			// Get all horizontal wall tiles (excluding corners by going from 1 to width -1)
			for (int i = 1; i < room.size.width - 1; i++) {
				Position southWallTilePosition = new Position(room.position.x + i, room.position.y);
				Position northWallTilePosition = new Position(room.position.x + i, room.position.y + room.size.height - 1);
				
				openingPossibilities.Add(new Opening(northWallTilePosition, Direction.North, "room"));
				openingPossibilities.Add(new Opening(southWallTilePosition, Direction.South, "room"));
			}

			for (int i = 1; i < room.size.height - 1; i++) {
				Position westWallTilePosition = new Position(room.position.x, room.position.y + i);
				Position eastWallTilePosition = new Position(room.position.x + room.size.width - 1, room.position.y + i);
				
				openingPossibilities.Add(new Opening(eastWallTilePosition, Direction.East, "room"));
				openingPossibilities.Add(new Opening(westWallTilePosition, Direction.West, "room"));
			}
				
		}
		
		// Please refactor me, I'm not DRY
		foreach (Tunnel tunnel in tunnels) {
			switch (tunnel.direction)
			{
				case Direction.North:

				// Sides of the tunnel
				for (int i = 1; i < tunnel.length - 1; i++) {
					Position eastWallTilePosition = new Position(tunnel.position.x + 1, tunnel.position.y + i);
					Position westWallTilePosition = new Position(tunnel.position.x - 1, tunnel.position.y + i);

					openingPossibilities.Add(new Opening(eastWallTilePosition, Direction.East, "tunnel"));
					openingPossibilities.Add(new Opening(westWallTilePosition, Direction.West, "tunnel"));
				}

				// End of the tunnel
				openingPossibilities.Add(new Opening(new Position(tunnel.position.x, tunnel.position.y + tunnel.length - 1), Direction.North, "tunnel"));
				break;

				case Direction.East:

				// Sides of the tunnel
				for (int i = 1; i < tunnel.length - 1; i++) {
					Position northWallTilePosition = new Position(tunnel.position.x + i, tunnel.position.y + 1);
					Position southWallTilePosition = new Position(tunnel.position.x + i, tunnel.position.y - 1);

					openingPossibilities.Add(new Opening(northWallTilePosition, Direction.North, "tunnel"));
					openingPossibilities.Add(new Opening(southWallTilePosition, Direction.South, "tunnel"));
				}

				// End of the tunnel
				openingPossibilities.Add(new Opening(new Position(tunnel.position.x + tunnel.length - 1, tunnel.position.y), Direction.East, "tunnel"));
				break;
				
				case Direction.South:

				// Sides of the tunnel
				for (int i = 1; i < tunnel.length - 1; i++) {
					Position eastWallTilePosition = new Position(tunnel.position.x + 1, tunnel.position.y - i);
					Position westWallTilePosition = new Position(tunnel.position.x - 1, tunnel.position.y - i);

					openingPossibilities.Add(new Opening(eastWallTilePosition, Direction.East, "tunnel"));
					openingPossibilities.Add(new Opening(westWallTilePosition, Direction.West, "tunnel"));
				}

				// End of the tunnel
				openingPossibilities.Add(new Opening(new Position(tunnel.position.x, tunnel.position.y - tunnel.length + 1), Direction.South, "tunnel"));
				break;
				
				case Direction.West:

				// Sides of the tunnel
				for (int i = 1; i < tunnel.length - 1; i++) {
					Position northWallTilePosition = new Position(tunnel.position.x - i, tunnel.position.y + 1);
					Position southWallTilePosition = new Position(tunnel.position.x - i, tunnel.position.y - 1);

					openingPossibilities.Add(new Opening(northWallTilePosition, Direction.North, "tunnel"));
					openingPossibilities.Add(new Opening(southWallTilePosition, Direction.South, "tunnel"));
				}

				// End of the tunnel
				openingPossibilities.Add(new Opening(new Position(tunnel.position.x - tunnel.length + 1, tunnel.position.y), Direction.West, "tunnel"));
				break;
				
				default:
				Debug.LogError("Tunnel's can't be made in diagonal directions");
				break;
			}
		}

	}
	public void AddDroppedItem(int id, Position position) {
		GameObject dropItem = Instantiate(droppedItem, Vector3.zero, Quaternion.identity);
		dropItem.GetComponent<DroppedItem>().Init(id, position);
		droppedItems.Add(dropItem);
	}

	void AddTunnel() {

		bool addedTunnel = false;
		int attemptCount = 0; // to prevent infinite loops

		// Keep trying to add a tunnel
		do {

			Opening randomOpeningPossibility = openingPossibilities[Random.Range(0, openingPossibilities.Count)];

			int tunnelLength = Random.Range(minimumTunnelLength, maximumTunnelLength);

			Size tunnelSize;
			Vector tunnelRectPositionDelta;
			RectangleArea tunnelArea;

			switch (randomOpeningPossibility.direction)
			{
				case Direction.North:
				tunnelSize = new Size(5, tunnelLength);
				tunnelRectPositionDelta = new Vector(-2, +1);
				tunnelArea = new RectangleArea(randomOpeningPossibility.position + tunnelRectPositionDelta, tunnelSize);
				break;
				
				case Direction.East:
				tunnelSize = new Size(tunnelLength, 5);
				tunnelRectPositionDelta = new Vector(+1, -2);
				tunnelArea = new RectangleArea(randomOpeningPossibility.position + tunnelRectPositionDelta, tunnelSize);
				break;
				
				case Direction.South:
				tunnelSize = new Size(5, tunnelLength);
				tunnelRectPositionDelta = new Vector(-2, -(tunnelSize.height));
				tunnelArea = new RectangleArea(randomOpeningPossibility.position + tunnelRectPositionDelta, tunnelSize);
				break;
				
				case Direction.West:	
				tunnelSize = new Size(tunnelLength, 5);
				tunnelRectPositionDelta = new Vector(-(tunnelSize.width), -2);
				tunnelArea = new RectangleArea(randomOpeningPossibility.position + tunnelRectPositionDelta, tunnelSize);
				break;
				
				default:
				Debug.LogError("Tunnel's can't be made in diagonal directions");
				tunnelSize = new Size(0,0);
				tunnelRectPositionDelta = new Vector(0,0);
				tunnelArea = new RectangleArea(new Position(-20, -29), new Size(-20, -20));
				break;
			}

			if (RectangleAreaIsEmpty(tunnelArea)) {
				tunnels.Add(new Tunnel(randomOpeningPossibility.position + randomOpeningPossibility.direction.ToVector(), tunnelLength, randomOpeningPossibility.direction));
				usedOpenings.Add(randomOpeningPossibility);
				addedTunnel = true;
			}

			attemptCount++;
			
		} while(!addedTunnel && attemptCount < 5);

	}

	void AddEnemyDefinition() {
		Room randomRoom = rooms[Random.Range(0, rooms.Count)];
		Position randomRoomTilePosition = new Position(randomRoom.position.x + 1 + Random.Range(0, (randomRoom.size.width - 2)), 
											   randomRoom.position.y + 1 + Random.Range(0, randomRoom.size.height - 2));
		enemyDefinitions.Add(new EnemyDefinition(randomRoomTilePosition));
	}

	void AddRoom() {

		bool addedRoom = false;
		int attemptCount = 0; // to prevent infinite loops

		do {
			List<Opening> tunnelOpeningPossibilities = GetOpeningsConnectedTo(openingPossibilities, "tunnel");
			Opening randomOpeningPossibility = tunnelOpeningPossibilities[Random.Range(0, tunnelOpeningPossibilities.Count)];

			Position openingPosition = randomOpeningPossibility.position;
			Direction openingDirection = randomOpeningPossibility.direction;

			Size roomSize = new Size(Random.Range(minimumRoomSize.width, maximumRoomSize.width), Random.Range(minimumRoomSize.height, maximumRoomSize.height));
			Position roomPosition;
			RectangleArea roomArea;

			switch (randomOpeningPossibility.direction) {
				case Direction.North:
				roomPosition = new Position(openingPosition.x - (roomSize.width / 2), openingPosition.y + 2);
				
				// with margin
				roomArea = new RectangleArea(new Position(roomPosition.x - 1, roomPosition.y + 1), 
											 new Size(roomSize.width + 2, roomSize.height + 2));

				break;

				case Direction.East:
				roomPosition = new Position(openingPosition.x + 2, openingPosition.y - (roomSize.height / 2));
				
				// with margin
				roomArea = new RectangleArea(new Position(roomPosition.x, roomPosition.y - 1), 
											 new Size(roomSize.width + 2, roomSize.height + 2));

				break;

				case Direction.South:
				roomPosition = new Position(openingPosition.x - (roomSize.width / 2), openingPosition.y - 1 - roomSize.height);

				// with margin
				roomArea = new RectangleArea(new Position(roomPosition.x - 1, roomPosition.y - 2), 
											 new Size(roomSize.width + 2, roomSize.height + 2));

				break;

				case Direction.West:
				roomPosition = new Position(openingPosition.x - 1 - roomSize.width, openingPosition.y - (roomSize.height / 2));
	
				// with margin
				roomArea = new RectangleArea(new Position(roomPosition.x - 2, roomPosition.y - 1), 
											 new Size(roomSize.width + 2, roomSize.height + 2));
				
				break;

				default:
				Debug.LogError("Openings's can't be made in diagonal directions");
				roomPosition = new Position(-1, -1);
				roomArea = new RectangleArea(new Position(-20, -29), new Size(-20, -20));
				break;
			}


			if (RectangleAreaIsEmpty(roomArea)) {
				rooms.Add(new Room(roomPosition, roomSize));
				usedOpenings.Add(randomOpeningPossibility);
				usedOpenings.Add(new Opening(openingPosition + openingDirection.ToVector(), openingDirection.Inverted(), randomOpeningPossibility.connectedTo));
				addedRoom = true;
			}
			attemptCount++;

		} while(!addedRoom && attemptCount < 5);
		
	}

	void AddChest() {
		
		bool addedChest = false;
		int attemptCount = 0; // to prevent infinite loops
		
		do {
			Room randomRoom = rooms[Random.Range(0, rooms.Count)];
			List<Position> possibleChestPositions = new List<Position>();
			
			for (int x = 1; x < randomRoom.size.width - 1; x++) {
				
				Position posBot = new Position (randomRoom.position.x + x, randomRoom.position.y + 1);
				
				if (tiles[posBot.x, posBot.y] == "ground" && !OpeningCloseToPosition(posBot)) {
					possibleChestPositions.Add(posBot);
				}

				Position posTop = new Position(randomRoom.position.x + x, randomRoom.position.y + randomRoom.size.height - 2);
				
				if (tiles[posTop.x, posTop.y] == "ground" && !OpeningCloseToPosition(posTop)) {
					possibleChestPositions.Add(posTop);
				}
			}
			
			for (int y = 1; y < randomRoom.size.height - 1; y++) {
				
				Position posLeft = new Position(randomRoom.position.x + 1, randomRoom.position.y + y);
				if (tiles[posLeft.x, posLeft.y] == "ground" && !OpeningCloseToPosition(posLeft)) {
					possibleChestPositions.Add(posLeft);
				}
				
				Position posRight = new Position(randomRoom.position.x + randomRoom.size.width - 2, randomRoom.position.y + y);
				if (tiles[posRight.x, posRight.y] == "ground" && !OpeningCloseToPosition(posRight)) {
					possibleChestPositions.Add(posRight);
				}
			}

			if (possibleChestPositions.Count != 0) {
				Position randomPosition = possibleChestPositions[Random.Range(0, possibleChestPositions.Count)];
				chests.Add(new Chest(randomPosition, GenerateRandomChestContents()));
				addedChest = true;
			}

			attemptCount++;
				
		} while (!addedChest && attemptCount < 5);
	}

	int[] GenerateRandomChestContents() {
		int[] items = new int[16];	
		return items;
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

	void AddChestToTiles(Chest chest) {
		tiles[chest.position.x, chest.position.y] = "chest";
	}
	
	public DroppedItem DroppedItemAtPosition(Position position) {
		foreach (GameObject droppedItem in droppedItems) {
			Position droppedItemPosition = droppedItem.GetComponent<DroppedItem>().position;
			if (position.x == droppedItemPosition.x && position.y == droppedItemPosition.y) {
				return droppedItem.GetComponent<DroppedItem>();
			}	
		}
		return null;
	}

	public int ChestAtPosition(Position position) {
		for (int i = 0; i < chests.Count; i++) {
			if (position.x == chests[i].position.x && position.y == chests[i].position.y) {
				return i;
			}
		}
		return -1; // no chest at the desired position;
	}

	public void RemoveDroppedItem(Position position) {
		for (int i = 0; i < droppedItems.Count; i++)
		{
			Position droppedItemPosition = droppedItems[i].GetComponent<DroppedItem>().position;
			if (droppedItemPosition.x == position.x && droppedItemPosition.y == position.y)  {
				GameObject.Destroy(droppedItems[i]);
				droppedItems.RemoveAt(i);
			}
		}
		
		
	}


	void AddUsedOpeningToTiles(Opening usedOpening) {

		tiles[usedOpening.position.x, usedOpening.position.y] = "ground";
		
		Dictionary<Direction, OffsetTilePair[]> offsetTilePairs = new Dictionary<Direction, OffsetTilePair[]>() {
			{ Direction.North, new OffsetTilePair[] { 
				new OffsetTilePair(new Vector(0, +1), "ground"), 
				new OffsetTilePair(new Vector(-1, +1), "wall"), 
				new OffsetTilePair(new Vector(+1, +1), "wall"), 
				new OffsetTilePair(new Vector(0, +2), "ground")} },
			{ Direction.East, new OffsetTilePair[]  { 
				new OffsetTilePair(new Vector(+1, 0), "ground"), 
				new OffsetTilePair(new Vector(+1, +1), "wall"), 
				new OffsetTilePair(new Vector(+1, -1), "wall"), 
				new OffsetTilePair(new Vector(+2, 0), "ground")} },
			{ Direction.South, new OffsetTilePair[] { 
				new OffsetTilePair(new Vector(0, -1), "ground"), 
				new OffsetTilePair(new Vector(-1, -1), "wall"), 
				new OffsetTilePair(new Vector(+1, -1), "wall"), 
				new OffsetTilePair(new Vector(0, -2), "ground")} },
			{ Direction.West, new OffsetTilePair[]  { 
				new OffsetTilePair(new Vector(-1, 0), "ground"), 
				new OffsetTilePair(new Vector(-1, -1), "wall"), 
				new OffsetTilePair(new Vector(-1, +1), "wall"), 
				new OffsetTilePair(new Vector(-2, 0), "ground")} 
				}
		};

		foreach (OffsetTilePair offsetTilePair in offsetTilePairs[usedOpening.direction]) {
			tiles[usedOpening.position.x + offsetTilePair.offset.dx,
 		          usedOpening.position.y + offsetTilePair.offset.dy] = offsetTilePair.tile;
		}

	}


	void AddTunnelToTiles(Tunnel tunnel) {

		Position openingPosition = tunnel.position - tunnel.direction.ToVector();
		
		tiles[openingPosition.x, openingPosition.y] = "ground";
		for (int i = 0; i < tunnel.length; i++) {
			{
				Dictionary<Direction, OffsetTilePair[]> offsetTilePairs = new Dictionary<Direction, OffsetTilePair[]>() {
					{Direction.North, new OffsetTilePair[] {
						new OffsetTilePair(new Vector(0, i), "ground"),
						new OffsetTilePair(new Vector(+1, i), "wall"),
						new OffsetTilePair(new Vector(-1, i), "wall")}},
					{Direction.East, new OffsetTilePair[] {
						new OffsetTilePair(new Vector(i, 0), "ground"),
						new OffsetTilePair(new Vector(i, +1), "wall"),
						new OffsetTilePair(new Vector(i, -1), "wall")}},
					{Direction.South, new OffsetTilePair[] {
						new OffsetTilePair(new Vector(0, -i), "ground"),
						new OffsetTilePair(new Vector(+1, -i), "wall"),
						new OffsetTilePair(new Vector(-1, -i), "wall")}},
					{Direction.West, new OffsetTilePair[] {
						new OffsetTilePair(new Vector(-i, 0), "ground"),
						new OffsetTilePair(new Vector(-i, +1), "wall"),
						new OffsetTilePair(new Vector(-i, -1), "wall") }}
				};
				
				foreach (OffsetTilePair offsetTilePair in offsetTilePairs[tunnel.direction]) {
					tiles[tunnel.position.x + offsetTilePair.offset.dx,
			  			  tunnel.position.y + offsetTilePair.offset.dy] = offsetTilePair.tile;
				}
			}
			
		}

		Position endPosition = new Position(tunnel.position.x + (tunnel.direction.ToVector().dx * (tunnel.length - 1)), 
											tunnel.position.y + (tunnel.direction.ToVector().dy * (tunnel.length - 1)));
		tiles[endPosition.x, endPosition.y] = "wall";
		
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

	
	/*void KillEnemies(){
		int i = 0;
		List<GameObject> templist = enemies;
		
		foreach (GameObject enemy in enemies) {
		Enemy script=enemy.GetComponent<Enemy>();
		i+=1;	
			if(script.curHealth<=0){
				templist.RemoveAt(i-1);
				Destroy(enemy);
			}
		}
		enemies = templist;
	}*/

	void Render() {
		RenderTiles ();
	}

	public void UpdateEnemies() {
		//KillEnemies();
		foreach (GameObject enemy in enemies) {
			enemy.GetComponent<Enemy>().AITurn();
		}
		
	}

	void RenderTiles() {
		for (int x = 0; x < tiles.GetLength(0); x++) {
			for (int y = 0; y < tiles.GetLength(1); y++) {

				string tileName = tiles [x, y];
				GameObject tile = groundTile;


				// Pick the correct wall tile based on the adjecent walls
				if (tileName == "wall") {
					tileName += ("-" +
					(tiles [x, y + 1] == "wall" ? "1" : "0") +
					(tiles [x + 1, y] == "wall" ? "1" : "0") +
					(tiles [x, y - 1] == "wall" ? "1" : "0") +
					(tiles [x - 1, y] == "wall" ? "1" : "0"));
				}

				tile.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("Sprites/Tiles/" + tileName);


				SpawnTile(tile, new Position(x, y));
			}
		}
	}

	List<Opening> GetOpeningsConnectedTo(List<Opening> openings, string connectedTo) {
		List<Opening> foundOpenings = new List<Opening>();
		foreach (Opening opening in openings) {
			if (opening.connectedTo == connectedTo) {
				foundOpenings.Add(opening);
			}
		}
		return foundOpenings;
	}


	// Used to prevent chests from spawning in or next to openings
	private bool OpeningCloseToPosition(Position position) {
		foreach (Opening usedOpening in usedOpenings) {
			if (( position.x == usedOpening.position.x && position.y == usedOpening.position.y ) ||
			
				( position.x + 1 == usedOpening.position.x && position.y == usedOpening.position.y ) ||
				( position.x + 2 == usedOpening.position.x && position.y == usedOpening.position.y ) ||
				( position.x - 1 == usedOpening.position.x && position.y == usedOpening.position.y ) ||
				( position.x - 2 == usedOpening.position.x && position.y == usedOpening.position.y ) ||
				
				( position.x == usedOpening.position.x && position.y + 1 == usedOpening.position.y ) ||
				( position.x == usedOpening.position.x && position.y + 2 == usedOpening.position.y ) ||
				( position.x == usedOpening.position.x && position.y - 1 == usedOpening.position.y ) ||
				( position.x == usedOpening.position.x && position.y - 2 == usedOpening.position.y ) ){

				return true;
			}
		}
		return false;
	}
		
	public bool CanMoveToTile(Position position) {
		return tiles [position.x, position.y] == "ground" 
		&& DroppedItemAtPosition(position) == null 
		&& EnemyIsOnTile(position) == null;
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
	class OffsetTilePair {
		public Vector offset;
		public string tile;

		public OffsetTilePair(Vector offset, string tile) {
			this.offset = offset;
			this.tile = tile;
		}
	}

}


