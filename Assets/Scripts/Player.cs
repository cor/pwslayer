using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player: MonoBehaviour {

	public Position position;
	public bool flipped;
	public int healthPoints;
	public int attackDamage;
	private int randomCrit;
	private int critChance;
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
	public void Move (Direction direction) {

		lastMoveDirection = direction;

		Level level = GameObject.FindWithTag("Level").GetComponent<Level>();

		if (level.CanMoveToTile(position + direction.ToVector())) {
			
			position += direction.ToVector ();

			GameObject enemy = level.EnemyIsOnTile (position);
			if (enemy != null) {
				enemy.SetActive(false);
			}
		}
		Enemy enemies = GameObject.FindWithTag ("Enemy").GetComponent <Enemy>();
		enemies.AITurn ();

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
