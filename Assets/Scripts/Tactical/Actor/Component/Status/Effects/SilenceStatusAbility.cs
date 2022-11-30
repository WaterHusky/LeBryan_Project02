using UnityEngine;
using Tactical.Actor.Component;
using Tactical.Actor.Model;
using Tactical.Battle.BattleState;

namespace Tactical.Actor.Component {

	/// <summary>
	/// A StatusEffect that prevents the target from using any Ability.
	///
	/// @todo Display a Status icon.
	/// </summary>
	public class SilenceStatusEffect : StatusEffect {

		private void OnEnable () {
			this.AddObserver(OnSilenceCheck, CommandCategorySelectionState.SilenceCheckNotification);
		}

		private void OnDisable () {
			this.RemoveObserver(OnSilenceCheck, CommandCategorySelectionState.SilenceCheckNotification);
		}

		/// <summary>
		/// Enables the status
		/// </summary>
		///
		/// <param name="sender">The sender.</param>
		/// <param name="args">The arguments.</param>
		private void OnSilenceCheck (object sender, object args) {
			var info = args as StatusInfo;
			var owner = GetComponentInParent<Unit>();

			if (info != null && info.target == owner) {
				info.enabled = true;
			}
		}

	}

}
