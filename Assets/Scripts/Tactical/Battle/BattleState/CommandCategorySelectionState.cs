using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Tactical.Core.Exceptions;
using Tactical.Actor.Component;

namespace Tactical.Battle.BattleState {

	/// <summary>
	/// A BattleState that allows the player to select a ability categories (AbilityCatalog).
	/// </summary>
	public class CommandCategorySelectionState : BaseActionMenuState {

		/// <summary>
		/// A notification sent before displaying each Ability category.
		/// </summary>
		public const string SilenceCheckNotification = "CommandCategorySelectionState.SilenceCheckNotification";

		public override void Enter () {
			base.Enter();

			// Display the unit's stats.
			statPanelController.ShowPrimary(turn.actor.gameObject);
		}

		public override void Exit () {
			base.Exit();

			// Hide the unit's stats.
			statPanelController.HidePrimary();
		}

		/// <summary>
		/// Loads the list of ability categories (+ Attack).
		/// </summary>
		protected override void LoadMenu () {
			menuTitle = "Abilities";

			if (menuOptions == null) {
				menuOptions = new List<string>();
			} else {
				menuOptions.Clear();
			}

			menuOptions.Add("Attack");

			var catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
			Assert.IsNotNull(catalog);

			for (int i = 0; i < catalog.CategoryCount(); ++i) {
				menuOptions.Add(catalog.GetCategory(i).name);
			}

			// Shows the menu.
			actionMenuPanelController.Show(menuTitle, menuOptions);

			for (int i = 1; i < menuOptions.Count + 1; ++i) {
				// If the player can't use abilites, disable the category.
				if (!CanUseAbilities()) {
					actionMenuPanelController.SetLocked(i, true);
				}
			}
		}

		protected override void Confirm () {
			// If the selected action is the first one, just Attack.
			if (actionMenuPanelController.selection == 0) {
				Attack();
			}
			// If it's a "normal" action, select the category.
			else {
				SetCategory(actionMenuPanelController.selection - 1);
			}
		}

		protected override void Cancel () {
			// Return to CommandSelectionState.
			owner.ChangeState<CommandSelectionState>();
		}

		/// <summary>
		/// Sets the ability to the first one (Attack) and go to AbilityTargetState.
		///
		/// @todo What if there is no Attack Ability?
		/// </summary>
		private void Attack () {
			turn.ability = turn.actor.GetComponentInChildren<Ability>();
			owner.ChangeState<AbilityTargetState>();
		}

		/// <summary>
		/// Selects the category and go to ActionSelectionState.
		/// </summary>
		///
		/// <param name="index">The index of the category.</param>
		private void SetCategory (int index) {
			ActionSelectionState.category = index;
			owner.ChangeState<ActionSelectionState>();
		}

		/// <summary>
		/// Determines if the turn actor can use abilities.
		/// </summary>
		///
		/// <returns>True if able to use abilities, False otherwise.</returns>
		private bool CanUseAbilities () {
			var exc = new MatchException(turn.actor, null);
			this.PostNotification(SilenceCheckNotification, exc);
			return !exc.toggle;
		}

	}

}
