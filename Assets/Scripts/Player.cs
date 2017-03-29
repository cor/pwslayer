using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player: MonoBehaviour {

	public Position position;
	public bool flipped;
	public int health;
	public int armor;

	// Animation
	public float smoothTime = 0.3f;
	private Vector3 velocity = Vector3.zero;
	private Direction lastMoveDirection = Direction.North;

	
	private Inventory inventory;
	private Level level;

	public void Start() {
		level = GameObject.Find("Level").GetComponent<Level>();
		inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
		position = new Position(level.size.width / 2, level.size.height / 2);
	}

	public void Combat (){
		EventLogger eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();
		InputManager inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();

		int attack = inventory.GetEquipedItem().Attack;
		int critChance = inventory.GetEquipedItem().Crit;
		
		int randomCrit = Random.Range (0, 100);
		int randomDamage = Random.Range (-1, 1);		
		int damage = randomCrit <= critChance ? attack * 2 : attack;

		inputManager.enemy.health -= Mathf.Max(0, damage - inputManager.enemy.definition.armor - randomDamage);
		level.UpdateEnemies();
		eventLogger.ToLog("you dealt: "+ ((Mathf.Max(0, damage - inputManager.enemy.definition.armor - randomDamage)).ToString()) +" dmg to " + inputManager.enemy.definition.name);
	}

	public void Move (Direction direction) {

		lastMoveDirection = direction;

		if (level.CanMoveToTile(position + direction.ToVector())) {
			
			UpdatePositionBy(direction.ToVector ());
			
		}
		else{
			Direction? newDirection = new Vector (direction.ToVector().dx, 0).ToDirection ();
			
			if (newDirection.HasValue) {
				if (level.CanMoveToTile (position + newDirection.Value.ToVector ())) {
					UpdatePositionBy(newDirection.Value.ToVector ());
				}
				else{
					Direction? newerDirection = new Vector (0, direction.ToVector().dy).ToDirection ();
					if (newerDirection.HasValue) {
						if (level.CanMoveToTile (position + newerDirection.Value.ToVector ())) {
							UpdatePositionBy(newerDirection.Value.ToVector ());
						}
					}
				}
			}
		}
		level.UpdateEnemies();
		
	}

	private void UpdatePositionBy(Vector vector) {
		position += vector;
		GetComponent<AudioSource>().Play();
	}
				
	void Update () {
		Render ();
	}

	public void Render() {

		// Update the GameObjects's position to represent the model's Position 
		Vector3 targetPosition = new Vector3 (position.x, position.y, transform.position.z);
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

		// Update scale to show flipped
		switch (lastMoveDirection) {
		case Direction.NorthEast:
		case Direction.East:
		case Direction.SouthEast:
			flipped = false;
			break;
		case Direction.SouthWest:
		case Direction.West:
		case Direction.NorthWest:
			flipped = true;
			break;
		}

		transform.localScale = new Vector3 (
			(flipped ? -1 : 1), 
			transform.localScale.y, 
			transform.localScale.z);
	}
}
