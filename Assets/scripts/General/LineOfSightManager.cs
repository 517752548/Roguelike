﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightManager : MonoBehaviour {

	private List<GameObject> allTiles = new List<GameObject>();

	private void TryActivateEnemy(GameObject tileGo) {
		Tile tile = tileGo.GetComponent<Tile>();

		if(tile.isVisible && tile.actor != null) {
			if(tile.actor.GetComponent<Enemy>() != null) {
				tile.actor.GetComponent<Enemy>().isActive = true;
				tile.actor.GetComponent<Enemy>().targetPosition = GetComponent<Player>().position;
			}
		}
	}

	public void CalculateLoS() {

		List<GameObject> tilesAroundPlayer = 
			DungeonGenerator.instance.GetTilesAroundPosition(
				GetComponent<Player>().position, 
				GetComponent<Player>().seeRadius);
		
		allTiles = DungeonGenerator.instance.GetTiles();

		// flag tiles that are next to player
		// as discovered and visible.
		foreach(GameObject tile in tilesAroundPlayer) {
			Tile t = tile.GetComponent<Tile>();
			t.isDiscovered = true;
			t.isVisible = true;
		}

		// flag tiles that are not in the radius of
		// player's sight to invisible.
		foreach(GameObject tile in allTiles) {
			if(tilesAroundPlayer.Contains(tile) == false) {
				tile.GetComponent<Tile>().isVisible = false;
			}
		}

		// calculates blocked LOS.
		CalculateWallLoS();

		EnemyActivation();

	}

	private void EnemyActivation() {
		foreach(GameObject tile in allTiles) {
			TryActivateEnemy(tile);
		}
	}

	private void CalculateWallLoS() {
		List<GameObject> hiddenTiles = new List<GameObject>();
		Vector2 myPos = GetComponent<Actor>().position;
		GameObject hiddenTile = null;

		/* TILES:
		 * - center is the player
		 * - eight tiles are tested
		 * - tiles behind walls are made invisible.
		 * 
		 *  1 1 2 3 3
		 *  1 1 2 3 3
		 *  4 4 # 5 5
		 *  6 6 7 8 8
		 *  6 6 7 8 8
		 */

		// 1.
		GameObject tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 1, myPos.y + 1));
		Tile tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 2, myPos.y + 1));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 2, myPos.y + 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 1, myPos.y + 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		// 2.
		tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x, myPos.y + 1));
		tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x, myPos.y + 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		// 3.
		tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 1, myPos.y + 1));
		tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 1, myPos.y + 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 2, myPos.y + 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 2, myPos.y + 1));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		// 4.
		tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 1, myPos.y));
		tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 2, myPos.y));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		// 5.
		tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 1, myPos.y));
		tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 2, myPos.y));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		// 6.
		tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 1, myPos.y - 1));
		tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 1, myPos.y - 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 2, myPos.y - 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x - 2, myPos.y - 1));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		// 7.
		tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x, myPos.y - 1));
		tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x, myPos.y - 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		// 8.
		tileGo = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 1, myPos.y - 1));
		tile = tileGo.GetComponent<Tile>();

		if(tile.myType == Tile.TileType.Wall || tile.myType == Tile.TileType.OuterWall || tile.myType == Tile.TileType.DoorClosed) {
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 1, myPos.y - 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 2, myPos.y - 2));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
			hiddenTile = DungeonGenerator.instance.GetTileAtPos(new Vector2(myPos.x + 2, myPos.y - 1));
			if(hiddenTile != null) hiddenTiles.Add(hiddenTile);
		}

		foreach(GameObject t in hiddenTiles) {
			t.GetComponent<Tile>().isVisible = false;
		}

	}

}
