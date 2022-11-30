using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Core.Exceptions;
using Tactical.Actor.Component;

namespace Tactical.Actor.Component {

	public class AbilityManaCost : MonoBehaviour {

		public int amount;
		private Ability owner;

		private void Awake () {
			owner = GetComponent<Ability>();
		}

		private void OnEnable () {
			this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
			this.AddObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
		}

		private void OnDisable () {
			this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
			this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
		}

		private void OnCanPerformCheck (object sender, object args) {
			Stats s = GetComponentInParent<Stats>();
			if (s[StatTypes.MP] < amount) {
				var exc = (BaseException)args;
				exc.FlipToggle();
			}
		}

		private void OnDidPerformNotification (object sender, object args) {
			Stats s = GetComponentInParent<Stats>();
			s[StatTypes.MP] -= amount;
		}

	}

}
