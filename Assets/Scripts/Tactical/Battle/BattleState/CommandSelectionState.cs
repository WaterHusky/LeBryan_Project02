using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core.Enums;

namespace Tactical.Battle.BattleState {

	public class CommandSelectionState : BaseActionMenuState {

		public override void Enter () {
			base.Enter();
			statPanelController.ShowPrimary(turn.actor.gameObject);

			if (driver.Current == Drivers.Computer) {
				StartCoroutine(ComputerTurn());
			}
		}

		public override void Exit () {
			base.Exit();
			statPanelController.HidePrimary();
		}

		protected override void LoadMenu () {
			if (menuOptions == null) {
				menuTitle = "Menu";
				menuOptions = new List<string>(3);
				menuOptions.Add("Move");
				menuOptions.Add("Action");
				menuOptions.Add("Wait");
			}

			actionMenuPanelController.Show(menuTitle, menuOptions);
			actionMenuPanelController.SetLocked(0, turn.hasUnitMoved);
			actionMenuPanelController.SetLocked(1, turn.hasUnitActed);
		}

		protected override void Confirm () {
			switch (actionMenuPanelController.selection) {
			case 0: // Move
				owner.ChangeState<MoveTargetState>();
				break;
			case 1: // Action
				owner.ChangeState<CommandCategorySelectionState>();
				break;
			case 2: // Wait
				owner.ChangeState<EndFacingState>();
				break;
			}
		}

		protected override void Cancel () {
			if (turn.hasUnitMoved && !turn.lockMove) {
				turn.UndoMove();
				actionMenuPanelController.SetLocked(0, false);
				SelectTile(turn.actor.tile.pos);
			} else {
				owner.ChangeState<ExploreState>();
			}
		}

		private IEnumerator ComputerTurn () {
			if (turn.plan == null) {
				turn.plan = owner.cpu.Evaluate();
				turn.ability = turn.plan.ability;
			}

			yield return new WaitForSeconds (0.5f);

			if (!turn.hasUnitMoved && turn.plan.moveLocation != turn.actor.tile.pos) {
				owner.ChangeState<MoveTargetState>();
			} else if (!turn.hasUnitActed && turn.plan.ability != null) {
				owner.ChangeState<AbilityTargetState>();
			} else {
				owner.ChangeState<EndFacingState>();
			}
		}

	}

}
