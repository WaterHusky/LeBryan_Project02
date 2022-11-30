using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.Controller;
using Tactical.Battle.Controller;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Actor.Component;
using Tactical.Actor.Factory;

namespace Tactical.Battle.BattleState {

	public class InitBattleState : BattleState {

		public override void Enter () {
			base.Enter();
			StartCoroutine("Init");
		}

		private IEnumerator Init () {
			// Load the board.
			board.Load(levelData);

			// Select the tile at [0,0].
			var p = new Point((int)levelData.tiles[0].x, (int) levelData.tiles[0].z);
			SelectTile(p);

			// Spawn test units.
			// TODO: Use a real system to spawn units depending on the situation.
			SpawnTestUnits();
			yield return null;
			owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();

			AddVictoryCondition();

			// Load the Cut scene state.
			owner.ChangeState<CutSceneState>();
		}

		private void SpawnTestUnits () {
			string[] recipes = {
				"Ramza",
				"Delita",
				"Ovelia",
				"Enemy Warrior",
				"Enemy Rogue",
				"Enemy Wizard"
			};

			var locations = new List<Tile>(board.tiles.Values);
			for (int i = 0; i < recipes.Length; ++i) {
				int level = UnityEngine.Random.Range(9, 12);
				GameObject instance = UnitFactory.Create(recipes[i], level);
				instance.transform.parent = owner.board.unitsContainer;

				int random = UnityEngine.Random.Range(0, locations.Count);
				Tile randomTile = locations[ random ];
				locations.RemoveAt(random);

				Unit unit = instance.GetComponent<Unit>();
				unit.Place(randomTile);
				unit.dir = (Directions) UnityEngine.Random.Range(0, 4);
				unit.Match();

				units.Add(unit);
			}

			SelectTile(units[0].tile.pos);
		}

		private void AddVictoryCondition () {
			DefeatTargetVictoryCondition vc = owner.gameObject.AddComponent<DefeatTargetVictoryCondition>();
			Unit enemy = units[units.Count - 1];
			if (enemy == null) {
				throw new Exception("Could not find any unit set as DefeatTargetVictoryCondition's target.");
			}
			vc.target = enemy;
		}

	}

}
