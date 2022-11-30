using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.EventArgs;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Actor.Component;

namespace Tactical.Battle.BattleState {

	public class ConfirmAbilityTargetState : BattleState {

		private List<Tile> tiles;
		private AbilityArea aa;
		private int index;

		public override void Enter () {
			base.Enter();
			aa = turn.ability.GetComponent<AbilityArea>();
			tiles = aa.GetTilesInArea(board, pos);
			board.SelectTiles(tiles);
			FindTargets();
			RefreshPrimaryStatPanel(turn.actor.tile.pos);

			if (turn.targets.Count > 0) {
				hitIndicatorPanelController.Show();
				SetTarget(0);
			}

			// Only show this UI for AI controlled units
			if (driver.Current == Drivers.Computer) {
				StartCoroutine(ComputerDisplayAbilitySelection());
			}
		}

		public override void Exit () {
			base.Exit();
			board.DeSelectTiles(tiles);
			statPanelController.HidePrimary();
			statPanelController.HideSecondary();
			hitIndicatorPanelController.Hide();
		}

		protected override void OnMove (object sender, InfoEventArgs<Point> e) {
			if (e.info.y > 0 || e.info.x > 0) {
				SetTarget(index + 1);
			} else {
				SetTarget(index - 1);
			}
		}

		protected override void OnAction (object sender, InfoEventArgs<BattleInputs> e) {
			if (e.info == BattleInputs.Confirm) {
				if (turn.targets.Count > 0) {
					owner.ChangeState<PerformAbilityState>();
				}
			} else if (e.info == BattleInputs.Cancel) {
				owner.ChangeState<AbilityTargetState>();
			}
		}

		private void FindTargets () {
			turn.targets = new List<Tile>();
			for (int i = 0; i < tiles.Count; ++i)
				if (IsTarget(tiles[i])) {
					turn.targets.Add(tiles[i]);
				}
		}

		private bool IsTarget (Tile tile) {
			Transform obj = turn.ability.transform;
			for (int i = 0; i < obj.childCount; ++i) {
				AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
				if (targeter.IsTarget(tile)) {
					return true;
				}
			}
			return false;
		}

		private void SetTarget (int target) {
			index = target;
			if (index < 0) {
				index = turn.targets.Count - 1;
			}
			if (index >= turn.targets.Count) {
				index = 0;
			}

			if (turn.targets.Count > 0) {
				RefreshSecondaryStatPanel(turn.targets[index].pos);
				UpdateHitSuccessIndicator ();
			}
		}

		private void UpdateHitSuccessIndicator () {
			int chance = 0;
			int amount = 0;
			Tile target = turn.targets[index];

			Transform obj = turn.ability.transform;
			for (int i = 0; i < obj.childCount; ++i) {
				AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
				if (targeter.IsTarget(target)) {
					HitRate hitRate = targeter.GetComponent<HitRate>();
					chance = hitRate.Calculate(target);

					BaseAbilityEffect effect = targeter.GetComponent<BaseAbilityEffect>();
					if (effect == null) {
						throw new Exception("Missing AbilityEffect component.");
					}
					amount = effect.Predict(target);
					break;
				}
			}

			hitIndicatorPanelController.SetStats(chance, amount);
		}

		private IEnumerator ComputerDisplayAbilitySelection () {
			owner.battleMessageController.Display(turn.ability.name);
			yield return new WaitForSeconds(1f);
			owner.ChangeState<PerformAbilityState>();
		}

	}

}
