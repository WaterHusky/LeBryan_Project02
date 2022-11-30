using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Tactical.Core.Exceptions;
using Tactical.Grid.Component;
using Tactical.Actor.Component;

namespace Tactical.Actor.Component {

	public class Ability : MonoBehaviour {

		public const string CanPerformCheck = "Ability.CanPerformCheck";
		public const string FailedNotification = "Ability.FailedNotification";
		public const string DidPerformNotification = "Ability.DidPerformNotification";

		private void OnValidate () {
			Assert.IsNotNull(GetComponent<AbilityRange>(), "Missing component: AbilityRange.");
			Assert.IsNotNull(GetComponent<AbilityArea>(), "Missing component: AbilityArea.");

			Assert.IsNotNull(GetComponentInChildren<BaseAbilityEffect>(), "Missing component: BaseAbilityEffect in Children.");
			Assert.IsNotNull(GetComponentInChildren<AbilityEffectTarget>(), "Missing component: AbilityEffectTarget in Children.");
			Assert.IsNotNull(GetComponentInChildren<HitRate>(), "Missing component: HitRate in Children.");
		}

		public bool CanPerform () {
			var exc = new BaseException(true);
			this.PostNotification(CanPerformCheck, exc);
			return exc.toggle;
		}

		public void Perform (List<Tile> targets, AudioSource audioSource) {
			if (!CanPerform()) {
				this.PostNotification(FailedNotification);
				return;
			}

			for (int i = 0; i < targets.Count; ++i) {
				Perform(targets[i], audioSource);
			}

			this.PostNotification(DidPerformNotification);
		}

		private void Perform (Tile target, AudioSource audioSource) {
			for (int i = 0; i < transform.childCount; ++i) {
				Transform child = transform.GetChild(i);
				var effect = child.GetComponent<BaseAbilityEffect>();
				effect.Apply(target, audioSource);
			}
		}

		public bool IsTarget (Tile tile) {
			Transform obj = transform;
			for (int i = 0; i < obj.childCount; ++i) {
				AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
				if (targeter.IsTarget(tile)) {
					return true;
				}
			}
			return false;
		}

	}

}
