using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// An BaseAbilityEffect to heal a target.
	///
	/// @todo Heal HP or MP.
	/// </summary>
	public class HealAbilityEffect : BaseAbilityEffect {

		/// <summary>
		/// Predicts the amount of healing done to the target.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The amount of heal predicted.</returns>
		public override int Predict (Tile target) {
			Unit attacker = GetComponentInParent<Unit>();
			Unit defender = target.content.GetComponent<Unit>();

			return GetStat(attacker, defender, GetPowerNotification, 0);
		}

		/// <summary>
		/// Heals the predicted amount after applying some random variance
		/// and standard clamping.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The final amount of heal.</returns>
		protected override int OnApply (Tile target) {
			Unit defender = target.content.GetComponent<Unit>();

			// Start with the predicted amount.
			int amount = Predict(target);

			// Add some random variance.
			amount = Mathf.FloorToInt(amount * Random.Range(0.9f, 1.1f));

			// Clamp the amount to a range.
			amount = Mathf.Clamp(amount, minDamage, maxDamage);

			// Apply the amount to the target.
			Stats s = defender.GetComponent<Stats>();
			s[StatTypes.HP] += amount;

			return amount;
		}

	}

}
