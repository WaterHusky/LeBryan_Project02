using UnityEngine;
using System.Collections;
using Tactical.Core.Enums;
using Tactical.Core.Extensions;
using Tactical.Core.EventArgs;
using Tactical.Grid.Model;

namespace Tactical.Battle.BattleState {

	public class EndFacingState : BattleState {

		private Directions startDir;

		public override void Enter () {
			base.Enter();

			startDir = turn.actor.dir;
			SelectTile(turn.actor.tile.pos);

			// Show the unit direction indicator.
			unitDirectionController.Show(startDir);

			// Computer turn
			if (driver.Current == Drivers.Computer) {
				StartCoroutine(ComputerControl());
			}
		}

		public override void Exit () {
			base.Exit();
			unitDirectionController.Hide();
		}

		protected override void OnMove (object sender, InfoEventArgs<Point> e) {
			turn.actor.dir = e.info.GetDirection();
			unitDirectionController.SetDirection(turn.actor.dir);
			turn.actor.Match();
		}

		protected override void OnAction (object sender, InfoEventArgs<BattleInputs> e) {
			base.OnAction(sender, e);

			switch (e.info) {
			case BattleInputs.Confirm:
				owner.ChangeState<SelectUnitState>();
				break;
			case BattleInputs.Cancel:
				turn.actor.dir = startDir;
				turn.actor.Match();
				owner.ChangeState<CommandSelectionState>();
				break;
			}
		}

		private IEnumerator ComputerControl () {
			yield return new WaitForSeconds(0.5f);
			turn.actor.dir = owner.cpu.DetermineEndFacingDirection();
			turn.actor.Match();
			owner.unitDirectionController.SetDirection(turn.actor.dir);
			yield return new WaitForSeconds(0.5f);
			owner.ChangeState<SelectUnitState>();
		}

	}

}
