using UnityEngine;
using System;
using System.Collections.Generic;
using Tactical.Actor.Component;

namespace Tactical.Battle.BattleState {

	public class ActionSelectionState : BaseActionMenuState {

		public static int category;

		private AbilityCatalog catalog;

		public override void Enter () {
			base.Enter();
			statPanelController.ShowPrimary(turn.actor.gameObject);
		}

		public override void Exit () {
			base.Exit();
			statPanelController.HidePrimary();
		}

		protected override void LoadMenu () {
			catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
			GameObject container = catalog.GetCategory(category);
			menuTitle = container.name;

			int count = catalog.AbilityCount(container);
			if (menuOptions == null) {
				menuOptions = new List<string>(count);
			} else {
				menuOptions.Clear();
			}

			var locks = new bool[count];
			for (int i = 0; i < count; ++i) {
				Ability ability = catalog.GetAbility(category, i);
				if (ability == null) {
					throw new Exception("Missing Ability component.");
				}
				AbilityManaCost cost = ability.GetComponent<AbilityManaCost>();
				if (cost) {
					menuOptions.Add(string.Format("{0}: {1}", ability.name, cost.amount));
				} else {
					menuOptions.Add(ability.name);
				}
				locks[i] = !ability.CanPerform();
			}

			actionMenuPanelController.Show(menuTitle, menuOptions);
			for (int i = 0; i < count; ++i) {
				actionMenuPanelController.SetLocked(i, locks[i]);
			}
		}

		protected override void Confirm () {
			turn.ability = catalog.GetAbility(category, actionMenuPanelController.selection);
			owner.ChangeState<AbilityTargetState>();
		}

		protected override void Cancel () {
			owner.ChangeState<CommandCategorySelectionState>();
		}

	}

}
