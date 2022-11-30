using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Core.Component;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// An BaseAbilityEffect to deal damage to a target.
	/// </summary>
	public class DamageAbilityEffect : BaseAbilityEffect {

		/// <summary>
		/// Predicts the amount of damage done to the target.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The amount of damage predicted.</returns>
		public override int Predict (Tile target) {
			Unit attacker = GetComponentInParent<Unit>();
			Unit defender = target.content.GetComponent<Unit>();

			// Get the attackers base attack stat considering mission items,
			// support check, status check, and equipment, etc.
			int attack = GetStat(attacker, defender, GetAttackNotification, 0);

			// Get the targets base defense stat considering mission items,
			// support check, status check, and equipment, etc.
			int defense = GetStat(attacker, defender, GetDefenseNotification, 0);

			// Calculate base damage.
			int damage = attack - (defense / 2);
			damage = Mathf.Max(damage, 1);

			// Get the abilities power stat considering possible variations.
			int power = GetStat(attacker, defender, GetPowerNotification, 0);

			// Apply power bonus.
			damage = power * damage / 100;
			damage = Mathf.Max(damage, 1);

			// Tweak the damage based on a variety of other checks like
			// Elemental damage, Critical Hits, Damage multipliers, etc.
			damage = GetStat(attacker, defender, TweakDamageNotification, damage);

			// Clamp the damage to a range.
			damage = Mathf.Clamp(damage, minDamage, maxDamage);
			return -damage;
		}

		/// <summary>
		/// Deals the predicted amount of damage after applying some random
		/// variance and standard clamping.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The final amount of damage dealt.</returns>
		protected override int OnApply (Tile target) {
			Unit defender = target.content.GetComponent<Unit>();

			// Start with the predicted damage amount.
			int amount = Predict(target);

			// Add some random variance.
			amount = Mathf.FloorToInt(amount * Random.Range(0.9f, 1.1f));

			// Clamp the damage to a range.
			amount = Mathf.Clamp(amount, minDamage, maxDamage);

			// Apply the damage to the target.
			Stats s = defender.GetComponent<Stats>();
			s[StatTypes.HP] += amount;
			Debug.Log(string.Format("Dealing {0} damage", amount), this);

			return amount;
		}

	}

}
