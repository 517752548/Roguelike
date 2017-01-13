﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {

	[Header("Enemy specific settings")]
	public string enemyDescription = "";

	[HideInInspector] public bool isActive = false;
	[HideInInspector] public Vector2 targetPosition = Vector2.zero;

	/// <summary>
	/// Finds the next tile.
	/// This is really simplified pathfinding 
	/// and should be upgraded to atleast to use
	/// pathfinding.cs (breadth-first search).
	/// </summary>
	/// <returns>The find next tile.</returns>
	private GameObject PathFindNextTile() {

		// Simple pathfinding algorithm
		// 1. get the tiles around the enemy.
		// 2. get candidate tiles (allowed to walk etc.)
		// 3. get the closest tile to player
		// 4. return that tile

		float shortestDistance = Mathf.Infinity;
		GameObject bestTile = null;

		foreach(GameObject tileGo in DungeonGenerator.instance.GetTilesAroundPosition(position, 1)) {
			Tile tile = tileGo.GetComponent<Tile>();

			// tile is walkable
			if(tile.myType == Tile.TileType.Floor ||
				tile.myType == Tile.TileType.DoorOpen ||
				tile.myType == Tile.TileType.FloorSpecialItem) {

				// tile is not occupied.
				if(tile.actor == null) {

					float distance = Vector2.Distance(tile.position, targetPosition);

					if(distance < shortestDistance) {
						bestTile = tileGo;
						shortestDistance = distance;
						myNextState = NextMoveState.Move;
					}

				} else {

					// if the player is near 
					// prioritize attacking.

					if(tile.actor.GetComponent<Player>() != null) {

						bestTile = tileGo;
						myNextState = NextMoveState.Attack;
						break;

					}
				}

				// if the player is standing on an exit tile
				// and this enemy can reach it -> attack.
			} else if(tile.myType == Tile.TileType.Exit) {

				if(tile.actor != null) {
					if(tile.actor.GetComponent<Player>() != null) {
						bestTile = tileGo;
						myNextState = NextMoveState.Attack;
						break;
					}
				}

			}
		}
		return bestTile;
	}

	private void CalculateNextStep() {
		GameObject bestTile = PathFindNextTile();
		if(bestTile != null) moveTargetPosition = bestTile.GetComponent<Tile>().position;
		else myNextState = NextMoveState.Pass;
	}

	public void DecideNextStep() {
		CalculateNextStep();

		if(myNextState == NextMoveState.Attack) {
			Attack();
		} else if(myNextState == NextMoveState.Move) {
			Move();
		} else if(myNextState == NextMoveState.Pass) {
			// pass the turn.
		}
	}

}