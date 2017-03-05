using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthbarUpdater : MonoBehaviour {


	public float healthBar;

	void Update () {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i <enemies.Length; i++){
			Enemy enemy = enemies[i].GetComponent<Enemy>();
			healthBar = (float)enemy.curHealth/(float)enemy.maxHealth;
			transform.localScale = new Vector3(healthBar,1,1);
		}
	}
}
