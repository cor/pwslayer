using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {
	private List<Item> database = new List<Item>();
	private JsonData itemData;

	void Start() {
		itemData = JsonMapper.ToObject (File.ReadAllText(Application.streamingAssetsPath + "/Items.json"));
		ConstructItemDatabase ();
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
				(int)itemData[i]["stats"]["attack"],
				(int)itemData[i]["stats"]["armor"],
				(int)itemData[i]["stats"]["crit"],
				(int)itemData[i]["stats"]["speed"],
				(int)itemData[i]["stats"]["range"],
				itemData[i]["description"].ToString(),
				(bool)itemData[i]["stackable"],
				itemData[i]["itemtype"].ToString(),
				itemData[i]["slug"].ToString()
			));
		
		}
	}
}

public class Item {

	public int ID { get; set; }
	public string Title { get; set; }
	public int Attack { get; set; }
	public int Armor { get; set; }
	public int Crit { get; set; }
	public int Speed { get; set; }
	public int Range { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
	public string Itemtype { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }


	public Item(int id, string title, int attack, int armor, int crit, int speed, int range, string description, bool stackable, string itemtype, string slug) {
		this.ID = id;
		this.Title = title;
		this.Attack = attack;
		this.Armor = armor;
		this.Crit = crit;
		this.Speed = speed;
		this.Range = range;
		this.Description = description;
		this.Stackable = stackable;
		this.Itemtype = itemtype;
		this.Slug = slug;

		this.Sprite = Resources.Load<Sprite> ("Sprites/Items/" + slug);
	}

	public Item() {
		this.ID = -1;
	}
}
