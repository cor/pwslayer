using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour{

	public EnemyDefinition definition;
	
    public Position position;
	public int health;


    // Animation
    public bool flipped;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private Direction lastMoveDirection = Direction.North;

	public bool playerInRange(){
		Player player = GameObject.Find("player").GetComponent<Player>();
		if ((player.position.x - position.x) <= 1 && 
		    (player.position.x - position.x) >= -1 && 
			(player.position.y - position.y) <= 1 && 
			(player.position.y - position.y) >= -1) {			
				
			return true;

		} else {			
			return false;
		}
	}
	
	public void AITurn()
	{
		EventLogger eventLogger = GameObject.Find("EventLog").GetComponent<EventLogger>();
		Player player = GameObject.Find ("player").GetComponent<Player> ();
		if (playerInRange ()) { //attack
			int randomCrit = Random.Range (0, 101); //random int to determine Crit
			int random = Random.Range (-1, 1); //random int to not have weapons deal set dmg
			int damage = (randomCrit <= definition.critChance ? definition.attack * 2 : definition.attack);
			player.health -= Mathf.Max(0, damage - player.armor - random);
			eventLogger.ToLog(definition.name + " dealt: " + ((Mathf.Max(0, damage - player.armor - random)).ToString()) + " dmg to you");
		} else { //move towards player
			int dx = player.position.x - position.x;
			int dy = player.position.y - position.y;
			Direction? direction = new Vector (dx, dy).ToDirection ();
			Level level = GameObject.FindWithTag ("Level").GetComponent<Level> ();
			if (direction.HasValue) {
				if (level.CanMoveToTile (position + direction.Value.ToVector ())) {
					position += direction.Value.ToVector ();
				}
				else{
					Direction? newDirection = new Vector (dx, 0).ToDirection ();
					if (newDirection.HasValue) {
						if (level.CanMoveToTile (position + newDirection.Value.ToVector ())) {
							position += newDirection.Value.ToVector ();
						}
						else{
							Direction? newerDirection = new Vector (0, dy).ToDirection ();
							if (newerDirection.HasValue) {
								if (level.CanMoveToTile (position + newerDirection.Value.ToVector ())) {
									position += newerDirection.Value.ToVector ();
								}
							}
						}
					}
				}		
			}
		}
	}

  
	public void Update(){
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