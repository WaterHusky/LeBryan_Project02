using UnityEngine;
using UnityEngine.Assertions;
using Tactical.Actor.Model;
using Tactical.Core.Enums;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// An BaseAbilityEffect to absorb some of the damage taken as HP.
	/// </summary>
	public class AbsorbDamageAbilityEffect : BaseAbilityEffect {

		/// <summary>
		/// A reference to the BaseAbilityEffect that is used to get the damage per example.
		/// </summary>
		private BaseAbilityEffect effect;

		/// <summary>
		/// The amount of damage to absorb.
		/// </summary>
		private int amount;

		protected override void Awake () {
			base.Awake();

			effect = GetTrackedEffect();
			Assert.IsNotNull(effect, "Missing property: effect.");
		}

		private void OnEnable () {
			this.AddObserver(OnEffectHit, BaseAbilityEffect.HitNotification, effect);
			this.AddObserver(OnEffectMiss, BaseAbilityEffect.MissNotification, effect);
		}

		private void OnDisable () {
			this.RemoveObserver(OnEffectHit, BaseAbilityEffect.HitNotification, effect);
			this.RemoveObserver(OnEffectMiss, BaseAbilityEffect.MissNotification, effect);
		}

		public override int Predict (Tile target) {
			return 0;
		}

		/// <summary>
		/// Heals the amount as HP.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>The amount of HP absorbed.</returns>
		protected override int OnApply (Tile target) {
			Stats s = GetComponentInParent<Stats>();
			s[StatTypes.HP] -= amount;
			Debug.Log(string.Format("Absorbing {0} damage", amount), this);

			return amount;
		}

		/// <summary>
		/// Sets the amount of HP to heal calculated from the damage done.
		///
		/// @todo Add the option to choose the percentage of damage absorbed.
		/// </summary>
		///
		/// <param name="sender">The sender.</param>
		/// <param name="args">The arguments.</param>
		private void OnEffectHit (object sender, object args) {
			var info = args as HitInfo;
			amount = (int) info.damage;
		}

		/// <summary>
		/// Sets the amount of HP to heal to 0 since the Ability missed the target.
		/// </summary>
		///
		/// <param name="sender">The sender.</param>
		/// <param name="damage">The damage done by the Ability.</param>
		private void OnEffectMiss (object sender, object damage) {
			amount = 0;
		}

		/// <summary>
		/// Gets the BaseAbilityEffect that is being tracked.
		/// </summary>
		///
		/// <returns>The tracked effect.</returns>
		private BaseAbilityEffect GetTrackedEffect () {
			Transform owner = GetComponentInParent<Ability>().transform;

			// FIXME: Is this useless?
			int trackedSiblingIndex = 0;

			if (trackedSiblingIndex >= 0 && trackedSiblingIndex < owner.childCount) {
				Transform sibling = owner.GetChild(trackedSiblingIndex);
				return sibling.GetComponent<BaseAbilityEffect>();
			}
			return null;
		}

	}

}
