using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyDefinition {

	public int id;
	public string name;
	public string slug;
	
	public int attack;
	public int maxHealth;
	public int critChance;

	public int armor;

	public EnemyDefinition(int id, string name, string slug, int attack, int maxHealth, int critChance, int armor) {
		this.id = id;
		this.name = name;
		this.slug = slug;
		this.attack = attack;
		this.maxHealth = maxHealth;
		this.critChance = critChance;
		this.armor = armor;
	}
}
