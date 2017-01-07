﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	public GameObject currentArmor;
	public GameObject currentWeapon;
	public GameObject currentUseItem;
	public GameObject currentSpell;

	[HideInInspector] public int currentGold = 0;

	public void HandleItem(GameObject itemGo) {

		// TODO
		// refactor.

		// 0.
		itemGo.GetComponent<Item>().owner = this.gameObject;

		// 1.
		if(itemGo.GetComponent<Armor>() != null) {
			
			if(currentArmor != null) {
				// swap the two items.
				DropCurrentItem(Item.Type.Armor);
				currentArmor = itemGo;
				currentArmor.GetComponent<Item>().HideItem();
				currentArmor.GetComponent<shadowController>().Hide();
			} else {
				// just pick up the item.
				DungeonGenerator.instance.UpdateTileItem(GetComponent<Actor>().position, null);
				currentArmor = itemGo;
				currentArmor.GetComponent<Item>().HideItem();
				currentArmor.GetComponent<shadowController>().Hide();
			}

		} else if(itemGo.GetComponent<Weapon>() != null) {

			if(currentWeapon != null) {
				// swap the two items.
				DropCurrentItem(Item.Type.Weapon);
				currentWeapon = itemGo;
				currentWeapon.GetComponent<Item>().HideItem();
				currentWeapon.GetComponent<shadowController>().Hide();
			} else {
				// just pick up the item.
				DungeonGenerator.instance.UpdateTileItem(GetComponent<Actor>().position, null);
				currentWeapon = itemGo;
				currentWeapon.GetComponent<Item>().HideItem();
				currentWeapon.GetComponent<shadowController>().Hide();
			}

		} else if(itemGo.GetComponent<Potion>() != null) {

			if(currentUseItem != null) {
				// swap the two items.
				DropCurrentItem(Item.Type.UsableItem);
				currentUseItem = itemGo;
				currentUseItem.GetComponent<Item>().HideItem();
				currentUseItem.GetComponent<shadowController>().Hide();
			} else {
				// just pick up the item.
				DungeonGenerator.instance.UpdateTileItem(GetComponent<Actor>().position, null);
				currentUseItem = itemGo;
				currentUseItem.GetComponent<Item>().HideItem();
				currentUseItem.GetComponent<shadowController>().Hide();
			}

		} else if(itemGo.GetComponent<Spell>() != null) {

			if(currentSpell != null) {
				// swap the two items.
				DropCurrentItem(Item.Type.Spell);
				currentSpell = itemGo;
				currentSpell.GetComponent<Item>().HideItem();
				currentSpell.GetComponent<shadowController>().Hide();
			} else {
				// just pick up the item.
				DungeonGenerator.instance.UpdateTileItem(GetComponent<Actor>().position, null);
				currentSpell = itemGo;
				currentSpell.GetComponent<Item>().HideItem();
				currentSpell.GetComponent<shadowController>().Hide();
			}

		} else if(itemGo.GetComponent<Gold>() != null) {

			int amount = itemGo.GetComponent<Gold>().amount;

			currentGold += amount;

			GUIManager.instance.CreatePopUpEntry("+" + amount + "g", GetComponent<Actor>().position, GUIManager.PopUpType.Gold);
			GUIManager.instance.CreateJournalEntry("Picked up " + amount + " gold.", GUIManager.JournalType.Item);

			DungeonGenerator.instance.UpdateTileItem(GetComponent<Actor>().position, null);
			Destroy(itemGo);

		}

		// 3.
		if(GetComponent<Player>() != null) GUIManager.instance.UpdateAllElements();

	}

	public void DropCurrentItem(Item.Type type) {

		Vector2 currentPosition = GetComponent<Actor>().position;
		Vector2 itemDropPosition = new Vector3(currentPosition.x, currentPosition.y, GameMaster.instance.itemZLevel);

		switch(type) {
		case Item.Type.Armor:
			currentArmor.GetComponent<Item>().ShowItem();
			currentArmor.transform.position = itemDropPosition;
			DungeonGenerator.instance.UpdateTileItem(currentPosition, currentArmor);
			currentArmor.GetComponent<Item>().owner = null;
			break;
		case Item.Type.Weapon:
			currentWeapon.GetComponent<Item>().ShowItem();
			currentWeapon.transform.position = itemDropPosition;
			DungeonGenerator.instance.UpdateTileItem(currentPosition, currentWeapon);
			currentWeapon.GetComponent<Item>().owner = null;
			break;
		case Item.Type.UsableItem:
			currentUseItem.GetComponent<Item>().ShowItem();
			currentUseItem.transform.position = itemDropPosition;
			DungeonGenerator.instance.UpdateTileItem(currentPosition, currentUseItem);
			currentUseItem.GetComponent<Item>().owner = null;
			break;
		case Item.Type.Spell:
			// reset spell's cooldown on drop,
			// so player can't scum multiple spells.
			currentSpell.GetComponent<Spell>().ResetCooldown();
			currentSpell.GetComponent<Item>().ShowItem();
			currentSpell.transform.position = itemDropPosition;
			DungeonGenerator.instance.UpdateTileItem(currentPosition, currentSpell);
			currentSpell.GetComponent<Item>().owner = null;
			break;
		}
	}
}
