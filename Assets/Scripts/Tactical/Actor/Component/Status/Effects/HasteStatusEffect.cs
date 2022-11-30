using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Core.Exceptions;

namespace Tactical.Actor.Component {

	public class HasteStatusEffect : StatusEffect {

		public float effectValue = 2f;

		private Stats myStats;

		private void OnEnable () {
			myStats = GetComponentInParent<Stats>();
			if (myStats) {
				this.AddObserver(OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats);
			}
		}

		private void OnDisable () {
			this.RemoveObserver(OnCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), myStats);
		}

		private void OnCounterWillChange (object sender, object args) {
			var exc = args as ValueChangeException;
			var m = new MultiplyDeltaModifier(0, effectValue);
			exc.AddModifier(m);
		}

	}

}
