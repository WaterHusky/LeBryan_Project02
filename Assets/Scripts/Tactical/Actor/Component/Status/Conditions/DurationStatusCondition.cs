using UnityEngine;
using Tactical.Battle.Controller;
using Tactical.Actor.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// A StatusCondition that removes itself after a certain duration (in turns);
	/// </summary>
	public class DurationStatusCondition : StatusCondition {

		/// <summary>
		/// The number of turns that the Status will be applied.
		/// </summary>
		public int duration = 10;

		private void OnEnable () {
			this.AddObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
		}

		private void OnDisable () {
			this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeganNotification);
		}

		/// <summary>
		/// Decreases the duration and remove the Status if the duration is over.
		/// </summary>
		///
		/// <param name="sender">The sender.</param>
		/// <param name="args">The arguments.</param>
		private void OnNewTurn (object sender, object args) {
			duration--;
			if (duration <= 0) {
				Remove();
			}
		}
	}

}
