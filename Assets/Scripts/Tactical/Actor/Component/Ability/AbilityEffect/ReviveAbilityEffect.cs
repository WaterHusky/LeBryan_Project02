using UnityEngine;
using UnityEngine.Assertions;
using Tactical.Core.Enums;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// An BaseAbilityEffect to revive an unit and heal a part of its HP.
	/// </summary>
	public class ReviveAbilityEffect : BaseAbilityEffect {

		/// <summary>
		/// The percentage of HP to heal.
		/// </summary>
		[Range(0,100)]
		public int percent;

		/// <summary>
		/// Predicts the amount of HP to heal after reviving the target.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The final amount of heal.</returns>
		public override int Predict (Tile target) {
			Stats s = target.content.GetComponent<Stats>();
			return Mathf.FloorToInt(s[StatTypes.MHP] * percent / 100);
		}

		/// <summary>
		/// Sets the HP ot the target to the the predicted amount.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The final HP or the target.</returns>
		protected override int OnApply (Tile target) {
			Stats s = target.content.GetComponent<Stats>();
			Assert.IsNotNull(s);

			// Set the
			int value = s[StatTypes.HP] = Predict(target);

			return value;
		}

	}

}
