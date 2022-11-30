using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Tactical.Core.Component;
using Tactical.Core.Exceptions;
using Tactical.Grid.Component;
using Tactical.Actor.Model;

namespace Tactical.Actor.Component {

	/// <summary>
	/// The base class for Ability effects.
	/// It keeps track of the min/max damage of the effect, sends notifications
	/// when needed, and predicts the damage done.
	/// </summary>
	public abstract class BaseAbilityEffect : MonoBehaviour {

		/// <summary>
		/// A notification sent before trying to get the Attack Stats.
		/// </summary>
		public const string GetAttackNotification = "BaseAbilityEffect.GetAttackNotification";
		/// <summary>
		/// A notification sent before trying to get the Defense Stats.
		/// </summary>
		public const string GetDefenseNotification = "BaseAbilityEffect.GetDefenseNotification";
		/// <summary>
		/// A notification sent before trying to get the Power Stats.
		/// </summary>
		public const string GetPowerNotification = "BaseAbilityEffect.GetPowerNotification";
		/// <summary>
		/// A notification sent when the Damage is being tweaked by external checks.
		/// </summary>
		public const string TweakDamageNotification = "BaseAbilityEffect.TweakDamageNotification";
		/// <summary>
		/// A notification sent when an Ability missed it's target.
		/// </summary>
		public const string MissNotification = "BaseAbilityEffect.MissNotification";
		/// <summary>
		/// A notification sent when an Ability hits it's target.
		/// </summary>
		public const string HitNotification = "BaseAbilityEffect.HitNotification";

		/// <summary>
		/// Predicts the damage/heal done to the given target.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The amount of damage/heal.</returns>
		public abstract int Predict (Tile target);

		/// <summary>
		/// The absolute minimum damage an Ability can do.
		/// </summary>
		protected const int minDamage = -99999;
		/// <summary>
		/// The absolute maximum damage an Ability can do.
		/// </summary>
		protected const int maxDamage = 99999;

		/// <summary>
		/// The effect target of the Ability.
		/// </summary>
		private AbilityEffectTarget abilityEffectTarget;

		protected virtual void Awake () {
			abilityEffectTarget = GetComponent<AbilityEffectTarget>();
			Assert.IsNotNull(abilityEffectTarget, "Missing component: AbilityEffectTarget.");
		}

		/// <summary>
		/// Applies the effect to the target if the target is valid.
		/// </summary>
		///
		/// <param name="target">The targeted tile.</param>
		/// <param name="audioSource">The audio source used to play ability sound effects.</param>
		public void Apply (Tile target, AudioSource audioSource) {
			// Check if the target is valid.
			if (!abilityEffectTarget.IsTarget(target)) {
				return;
			}

			// Get the HitRate and roll for hit.
			var hitRate = GetComponent<HitRate>();
			Assert.IsNotNull(hitRate);
			if (hitRate.RollForHit(target)) {
				// TODO: Wait for the animation to finish before moving the camera
				//       back to the caster.
				int damage = OnApply(target);
				var info = new HitInfo(target, damage, audioSource);
				this.PostNotification(HitNotification, info);
			} else {
				var info = new MissInfo(target, audioSource);
				this.PostNotification(MissNotification, info);
			}
		}

		protected abstract int OnApply (Tile target);

		/// <summary>
		/// Gets a Stats value after applying all modifiers (ValueModifier).
		/// </summary>
		///
		/// <param name="attacker">The attacker.</param>
		/// <param name="target">The target.</param>
		/// <param name="notification">The notification to post.</param>
		/// <param name="startValue">The start value.</param>
		///
		/// <returns>The new value.</returns>
		protected virtual int GetStat (Unit attacker, Unit target, string notification, int startValue) {

			// Get the modifiers.
			var mods = new List<ValueModifier>();
			var info = new BaseAbilityEffectInfo(attacker, target, mods);
			this.PostNotification(notification, info);
			mods.Sort(Compare);

			// Apply all the modifiers.
			float value = startValue;
			for (int i = 0; i < mods.Count; ++i) {
				value = mods[i].Modify(startValue, value);
			}

			// Round the value and clamp it using max heal/damage consts.
			int retValue = Mathf.FloorToInt(value);
			retValue = Mathf.Clamp(retValue, minDamage, maxDamage);

			return retValue;
		}

		/// <summary>
		/// Compares two modifiers and returns a integer to order them.
		/// </summary>
		///
		/// <param name="mod1">First modifier.</param>
		/// <param name="mod2">Second modifier.</param>
		///
		/// <returns>The order value.</returns>
		private int Compare (ValueModifier mod1, ValueModifier mod2) {
			return mod1.sortOrder.CompareTo(mod2.sortOrder);
		}

	}

}
