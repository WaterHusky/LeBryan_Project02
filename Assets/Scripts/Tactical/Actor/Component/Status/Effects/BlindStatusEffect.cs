using UnityEngine;
using Tactical.Actor.Component;
using Tactical.Actor.Model;

namespace Tactical.Actor.Component {

	/// <summary>
	/// A StatusEffect that lowers the HitRate of the target.
	///
	/// @todo Display a Status icon.
	/// </summary>
	public class BlindStatusEffect : StatusEffect {

		private void OnEnable () {
			this.AddObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
		}

		private void OnDisable () {
			this.RemoveObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
		}

		/// <summary>
		/// Increses or decreases the rate depending on what unit is blind.
		/// </summary>
		///
		/// <param name="sender">The sender.</param>
		/// <param name="args">The arguments.</param>
		private void OnHitRateStatusCheck (object sender, object args) {
			var info = args as HitRateInfo;
			Unit owner = GetComponentInParent<Unit>();

			if (owner == info.attacker) {
				// The attacker is blind
				info.rate += 50;
			} else if (owner == info.target) {
				// The defender is blind
				info.rate -= 20;
			}
		}

	}

}
