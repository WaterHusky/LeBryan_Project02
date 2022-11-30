using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core;
using Tactical.Core.Enums;
using Tactical.Core.Component;
using Tactical.Actor.Component;
using Tactical.Item.Component;

public class ItemsTests : MonoBehaviour {

	List<GameObject> inventory = new List<GameObject>();
	List<GameObject> combatants = new List<GameObject>();

	private void Start () {
		CreateItems();
		CreateCombatants();
		StartCoroutine(SimulateBattle());
	}

	private void OnEnable () {
		this.AddObserver(OnEquippedItem, Equipment.EquippedNotification);
		this.AddObserver(OnUnEquippedItem, Equipment.UnEquippedNotification);
	}

	private void OnDisable () {
		this.RemoveObserver(OnEquippedItem, Equipment.EquippedNotification);
		this.RemoveObserver(OnUnEquippedItem, Equipment.UnEquippedNotification);
	}

	private void OnEquippedItem (object sender, object args) {
		Equipment eq = sender as Equipment;
		Equippable item = args as Equippable;
		inventory.Remove(item.gameObject);
		string message = string.Format("{0} equipped {1}", eq.name, item.name);
		Debug.Log(message, this);
	}

	private void OnUnEquippedItem (object sender, object args) {
		Equipment eq = sender as Equipment;
		Equippable item = args as Equippable;
		inventory.Add(item.gameObject);
		string message = string.Format("{0} un-equipped {1}", eq.name, item.name);
		Debug.Log(message, this);
	}

	GameObject CreateItem (string title, StatTypes type, int amount) {
		GameObject item = new GameObject(title);
		StatModifierFeature smf = item.AddComponent<StatModifierFeature>();
		smf.type = type;
		smf.amount = amount;
		return item;
	}

	GameObject CreateConsumableItem (string title, StatTypes type, int amount) {
		GameObject item = CreateItem(title, type, amount);
		item.AddComponent<Consumable>();
		return item;
	}

	GameObject CreateEquippableItem (string title, StatTypes type, int amount, EquipSlots slot) {
		GameObject item = CreateItem(title, type, amount);
		Equippable equip = item.AddComponent<Equippable>();
		equip.defaultSlots = slot;
		return item;
	}

	GameObject CreateHero () {
		GameObject actor = CreateActor("Hero");
		actor.AddComponent<Equipment>();
		return actor;
	}

	GameObject CreateActor (string title) {
		GameObject actor = new GameObject(title);
		Stats s = actor.AddComponent<Stats>();
		s[StatTypes.HP] = s[StatTypes.MHP] = UnityEngine.Random.Range(500, 1000);
		s[StatTypes.ATK] = UnityEngine.Random.Range(30, 50);
		s[StatTypes.DEF] = UnityEngine.Random.Range(30, 50);
		return actor;
	}

	private void CreateItems () {
		inventory.Add( CreateConsumableItem("Health Potion", StatTypes.HP, 300) );
		inventory.Add( CreateConsumableItem("Bomb", StatTypes.HP, -150) );
		inventory.Add( CreateEquippableItem("Sword", StatTypes.ATK, 10, EquipSlots.Primary) );
		inventory.Add( CreateEquippableItem("Broad Sword", StatTypes.ATK, 15, (EquipSlots.Primary | EquipSlots.Secondary)) );
		inventory.Add( CreateEquippableItem("Shield", StatTypes.DEF, 10, EquipSlots.Secondary) );
	}

	private void CreateCombatants () {
		combatants.Add( CreateHero() );
		combatants.Add( CreateActor("Monster") );
	}

	private IEnumerator SimulateBattle () {
		while (VictoryCheck() == false) {
			LogCombatants();
			HeroTurn();
			EnemyTurn();
			yield return new WaitForSeconds(0.1f);
		}
		LogCombatants();
		Debug.Log("Battle Completed", this);
	}

	private void HeroTurn () {
		int rnd = UnityEngine.Random.Range(0, 2);
		switch (rnd) {
		case 0:
			Attack(combatants[0], combatants[1]);
			break;
		default:
			UseInventory();
			break;
		}
	}

	private void EnemyTurn () {
		Attack(combatants[1], combatants[0]);
	}

	private void Attack (GameObject attacker, GameObject defender) {
		Stats s1 = attacker.GetComponent<Stats>();
		Stats s2 = defender.GetComponent<Stats>();
		int damage = Mathf.FloorToInt((s1[StatTypes.ATK] * 4 - s2[StatTypes.DEF] * 2) * UnityEngine.Random.Range(0.9f, 1.1f));
		s2[StatTypes.HP] -= damage;
		string message = string.Format("{0} hits {1} for {2} damage!", attacker.name, defender.name, damage);
		Debug.Log(message, this);
	}

	private void UseInventory () {
		int rnd = UnityEngine.Random.Range(0, inventory.Count);

		GameObject item = inventory[rnd];
		if (item.GetComponent<Consumable>() != null) {
			ConsumeItem(item);
		} else {
			EquipItem(item);
		}
	}

	private void ConsumeItem (GameObject item) {
		inventory.Remove(item);
		// This is dummy code - a user would know how to use an item and who to target with it
		StatModifierFeature smf = item.GetComponent<StatModifierFeature>();
		if (smf.amount > 0) {
			item.GetComponent<Consumable>().Consume( combatants[0] );
			Debug.Log("Ah... a potion!", this);
		} else {
			item.GetComponent<Consumable>().Consume( combatants[1] );
			Debug.Log("Take this you stupid monster!", this);
		}
	}

	private void EquipItem (GameObject item) {
		Debug.Log("Perhaps this will help...", this);
		Equippable toEquip = item.GetComponent<Equippable>();
		Equipment equipment = combatants[0].GetComponent<Equipment>();
		equipment.Equip (toEquip, toEquip.defaultSlots );
	}

	private bool VictoryCheck () {
		for (int i = 0; i < 2; ++i) {
			Stats s = combatants[i].GetComponent<Stats>();
			if (s[StatTypes.HP] <= 0) {
				return true;
			}
		}
		return false;
	}

	private void LogCombatants () {
		for (int i = 0; i < 2; ++i) {
			LogToConsole( combatants[i] );
		}
	}

	private void LogToConsole (GameObject actor) {
		Stats s = actor.GetComponent<Stats>();
		string message = string.Format("Name:{0} HP:{1}/{2} ATK:{3} DEF:{4}", actor.name, s[StatTypes.HP], s[StatTypes.MHP], s[StatTypes.ATK], s[StatTypes.DEF]);
		Debug.Log( message , this);
	}

}
