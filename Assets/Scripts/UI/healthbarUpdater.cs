using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthbarUpdater : MonoBehaviour {

public Enemy enemy;
public HealthBar healthBar;
	
	void Update (){
		GameObject[] healthBars = GameObject.FindGameObjectsWithTag("HealthBar");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemies.Length; i++)
        {
			enemy = enemies[i].GetComponent<Enemy>();
			healthBar = healthBars[i].GetComponent<HealthBar>();
			healthBar.HealthRemaining = ((float)enemy.health)/((float)enemy.definition.maxHealth);
            
        }
	}
}
