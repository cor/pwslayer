﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {
	private List<Item> database = new List<Item>();
	private JsonData itemData;

	void Start() {
		itemData = JsonMapper.ToObject (File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		ConstructItemDatabase ();

		Debug.Log (FetchItemByID(0).Slug);
	}

	public Item FetchItemByID(int id) {

		for (int i = 0; i < database.Count; i++) {
			
			if (database [i].ID == id) {
				return database [i];
			}
		}

		return null;
	}

	void ConstructItemDatabase() {
		for (int i = 0; i < itemData.Count; i++) {
			
			database.Add (new Item (
				(int)itemData[i]["id"], 
				itemData[i]["title"].ToString(), 
				(int)itemData[i]["value"],
				(int)itemData[i]["stats"]["power"],
				(int)itemData[i]["stats"]["defence"],
				(int)itemData[i]["stats"]["vitality"],
				itemData[i]["description"].ToString(),
				(bool)itemData[i]["stackable"],
				(int)itemData[i]["rarity"],
				itemData[i]["slug"].ToString()
			));
		
		}
	}
}

public class Item {

	public int ID { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
	public int Power { get; set; }
	public int Defence { get; set; }
	public int Vitality { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
	public int Rarity { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }


	public Item(int id, string title, int value, int power, int defence, int vitality, string description, bool stackable, int rarity, string slug) {
		this.ID = id;
		this.Title = title;
		this.Value = value;
		this.Power = power;
		this.Defence = defence;
		this.Vitality = vitality;
		this.Description = description;
		this.Stackable = stackable;
		this.Rarity = rarity;
		this.Slug = slug;

		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);
	}

	public Item() {
		this.ID = -1;
	}
}
