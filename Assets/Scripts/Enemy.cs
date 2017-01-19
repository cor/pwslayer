using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour{


    public Position position;
    public bool flipped;
    public int healthPoints;
	public int atkDmg;
	public int critChance;
	private int randCrit;
	private int rand;
    // Animation
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private Direction lastMoveDirection = Direction.North;

	//AITurn options
	private int dx;
	private int dy;
	public bool playerInRange(){
		Player player = GameObject.Find("player").GetComponent<Player>();
		if ((player.position.x - position.x)<= 1 && (player.position.x - position.x)>= -1 && (player.position.y - position.y)<= 1 && (player.position.y - position.y) >= -1) {			
			return true;

		} else {			
			return false;

		}
	}

	public void AITurn()
	{
		Player player = GameObject.Find ("player").GetComponent<Player> ();
		if (playerInRange ()) { //attack
			Debug.Log ("In range");
			randCrit = Random.Range (0, 101); //random int to determine Crit
			rand = Random.Range (-1, 1); //random int to not have weapons deal set dmg
			if (randCrit <= critChance) { //deal critical dmg to player
				player.healthPoints = player.healthPoints - (atkDmg * 2) - rand;
			} else { //deal normal dmg to player
				player.healthPoints = player.healthPoints - atkDmg - rand;
			}
		} else { //move towards player
			Debug.Log ("Out of range");
			dx = player.position.x - position.x;
			dy = player.position.y - position.y;
			Direction? direction = new Vector (dx, dy).ToDirection ();
			Level level = GameObject.FindWithTag ("Level").GetComponent<Level> ();
			if (direction.HasValue) {
				if (level.CanMoveToTile (position + direction.Value.ToVector ())) {
					position += direction.Value.ToVector ();
				}
			}
		}
	}
    

    void Update()
    {
        Render();
    }

    public void Render()
    {

        // Update the GameObjects's position to represent the model's Position 
        Vector3 targetPosition = new Vector3(position.x, position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Update scale to show flipped
        switch (lastMoveDirection)
        {
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

        transform.localScale = new Vector3(
            (flipped ? -1 : 1),
            transform.localScale.y,
            transform.localScale.z);
    }
}