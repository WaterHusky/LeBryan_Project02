using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.Extensions;
using Tactical.Core.EventArgs;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Actor.Component;

namespace Tactical.Battle.BattleState {

	public class AbilityTargetState : BattleState {

		private List<Tile> tiles;
		private AbilityRange ar;

		public override void Enter () {
			base.Enter();
			ar = turn.ability.GetComponent<AbilityRange>();
			SelectTiles();
			statPanelController.ShowPrimary(turn.actor.gameObject);
			if (ar.directionOriented) {
				RefreshSecondaryStatPanel(pos);
			}

			// Computer turn
			if (driver.Current == Drivers.Computer) {
				StartCoroutine(ComputerHighlightTarget());
			}
		}

		public override void Exit () {
			base.Exit ();
			board.DeSelectTiles(tiles);
			statPanelController.HidePrimary();
			statPanelController.HideSecondary();
		}

		protected override void OnMove (object sender, InfoEventArgs<Point> e) {
			if (ar.directionOriented) {
				ChangeDirection(e.info);
			} else {
				SelectTile(e.info + pos);
				RefreshSecondaryStatPanel(pos);
			}
		}

		protected override void OnAction (object sender, InfoEventArgs<BattleInputs> e) {
			base.OnAction(sender, e);

			if (e.info == BattleInputs.Confirm) {
				if (ar.directionOriented || tiles.Contains(board.GetTile(pos))) {
					owner.ChangeState<ConfirmAbilityTargetState>();
				}
			} else if (e.info == BattleInputs.Cancel) {
				owner.ChangeState<CommandCategorySelectionState>();
			}
		}

		private void ChangeDirection (Point p) {
			Directions dir = p.GetDirection();
			if (turn.actor.dir != dir) {
				board.DeSelectTiles(tiles);
				turn.actor.dir = dir;
				turn.actor.Match();
				SelectTiles ();
			}
		}

		private void SelectTiles () {
			tiles = ar.GetTilesInRange(board);
			board.SelectTiles(tiles);
		}

		private IEnumerator ComputerHighlightTarget () {
			if (ar.directionOriented) {
				ChangeDirection(turn.plan.attackDirection.GetNormal());
				yield return new WaitForSeconds(0.25f);
			} else {
				Point cursorPos = pos;
				while (cursorPos != turn.plan.fireLocation) {
					if (cursorPos.x < turn.plan.fireLocation.x) { cursorPos.x++; }
					if (cursorPos.x > turn.plan.fireLocation.x) { cursorPos.x--; }
					if (cursorPos.y < turn.plan.fireLocation.y) { cursorPos.y++; }
					if (cursorPos.y > turn.plan.fireLocation.y) { cursorPos.y--; }
					SelectTile(cursorPos);
					yield return new WaitForSeconds(0.15f);
				}
			}
			yield return new WaitForSeconds(0.5f);
			owner.ChangeState<ConfirmAbilityTargetState>();
		}

	}

}
