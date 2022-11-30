using UnityEngine;
using Tactical.Core.Exceptions;
using Tactical.Actor.Model;

namespace Tactical.Actor.Component {

	public abstract class BaseAbilityPower : MonoBehaviour {

		protected abstract int GetBaseAttack ();
		protected abstract int GetBaseDefense (Unit target);
		protected abstract int GetPower ();

		private void OnEnable () {
			this.AddObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackNotification);
			this.AddObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseNotification);
			this.AddObserver(OnGetPower, BaseAbilityEffect.GetPowerNotification);
		}

		private void OnDisable () {
			this.RemoveObserver(OnGetBaseAttack, BaseAbilityEffect.GetAttackNotification);
			this.RemoveObserver(OnGetBaseDefense, BaseAbilityEffect.GetDefenseNotification);
			this.RemoveObserver(OnGetPower, BaseAbilityEffect.GetPowerNotification);
		}

		private void OnGetBaseAttack (object sender, object args) {
			if (IsMyEffect(sender)) {
				var info = args as BaseAbilityEffectInfo;
				info.mods.Add( new AddValueModifier(0, GetBaseAttack()) );
			}
		}

		private void OnGetBaseDefense (object sender, object args) {
			if (IsMyEffect(sender)) {
				var info = args as BaseAbilityEffectInfo;
				info.mods.Add( new AddValueModifier(0, GetBaseDefense(info.target)) );
			}
		}

		private void OnGetPower (object sender, object args) {
			if (IsMyEffect(sender)) {
				var info = args as BaseAbilityEffectInfo;
				info.mods.Add( new AddValueModifier(0, GetPower()) );
			}
		}

		private bool IsMyEffect (object sender) {
			var obj = sender as MonoBehaviour;
			return (obj != null && obj.transform.parent == transform);
		}

	}

}
