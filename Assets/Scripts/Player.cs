using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player: MonoBehaviour {

	public Position position;
	public bool flipped;
	public int healthPoints;
	public int attackDamage;
	private int randomCrit;
	public int critChance;
	private int random;
	// Animation
	public float smoothTime = 0.3f;
	private Vector3 velocity = Vector3.zero;
	private Direction lastMoveDirection = Direction.North;

	public void Combat(){
		Enemy enemy = GameObject.Find("Enemy").GetComponent<Enemy>();
		randomCrit = Random.Range (0, 101); //random int to determine Crit
		random = Random.Range (-1, 1); //random int to not have weapons deal set dmg
		if (randomCrit <= critChance) { //deal critical dmg to player
			enemy.healthPoints -= (attackDamage * 2) - random;
		} else { //deal normal dmg to player
			enemy.healthPoints -= attackDamage - random;			
		}
		enemy.AITurn();
	}

	public void Start() {
		Level level = GameObject.FindWithTag("Level").GetComponent<Level>();
		position = new Position(level.size.width / 2, level.size.height / 2);
	}
	public void Move (Direction direction) {

		lastMoveDirection = direction;

		Level level = GameObject.FindWithTag("Level").GetComponent<Level>();

		if (level.CanMoveToTile(position + direction.ToVector())) {
			
			position += direction.ToVector ();

			
		}
		else{
				
				Direction? newDirection = new Vector (direction.ToVector().dx, 0).ToDirection ();
				if (newDirection.HasValue) {
					if (level.CanMoveToTile (position + newDirection.Value.ToVector ())) {
						position += newDirection.Value.ToVector ();
					}
					else{
						Direction? newerDirection = new Vector (0, direction.ToVector().dy).ToDirection ();
						if (newerDirection.HasValue) {
							if (level.CanMoveToTile (position + newerDirection.Value.ToVector ())) {
								position += newerDirection.Value.ToVector ();
							}
						}
					}
				}
			}
		level.UpdateEnemies();
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
