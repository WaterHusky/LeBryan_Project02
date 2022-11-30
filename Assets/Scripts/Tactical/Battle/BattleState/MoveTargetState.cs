using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.EventArgs;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Actor.Component;

namespace Tactical.Battle.BattleState {

	public class MoveTargetState : BattleState {

		private List<Tile> tiles;

		public override void Enter () {
			base.Enter();
			Movement mover = owner.turn.actor.GetComponent<Movement>();
			tiles = mover.GetTilesInRange(board);
			board.SelectTiles(tiles);
			RefreshPrimaryStatPanel(pos);

			// Computer turn
			if (driver.Current == Drivers.Computer) {
				StartCoroutine(ComputerHighlightMoveTarget());
			}
		}

		public override void Exit () {
			base.Exit();
			board.DeSelectTiles(tiles);
			tiles = null;
			statPanelController.HidePrimary();
		}

		protected override void OnMove (object sender, InfoEventArgs<Point> e) {
			SelectTile(e.info + pos);
			RefreshPrimaryStatPanel(pos);
		}

		protected override void OnAction (object sender, InfoEventArgs<BattleInputs> e) {
			base.OnAction(sender, e);

			if (e.info == BattleInputs.Confirm) {
				if (tiles.Contains(owner.currentTile)) {
					owner.ChangeState<MoveSequenceState>();
				}
			} else if (e.info == BattleInputs.Cancel) {
				owner.ChangeState<CommandSelectionState>();
			}
		}

		private IEnumerator ComputerHighlightMoveTarget () {
			Point cursorPos = pos;
			while (cursorPos != turn.plan.moveLocation) {
				if (cursorPos.x < turn.plan.moveLocation.x) { cursorPos.x++; }
				if (cursorPos.x > turn.plan.moveLocation.x) { cursorPos.x--; }
				if (cursorPos.y < turn.plan.moveLocation.y) { cursorPos.y++; }
				if (cursorPos.y > turn.plan.moveLocation.y) { cursorPos.y--; }
				SelectTile(cursorPos);
				yield return new WaitForSeconds(0.15f);
			}
			yield return new WaitForSeconds(0.5f);
			owner.ChangeState<MoveSequenceState>();
		}

	}

}

