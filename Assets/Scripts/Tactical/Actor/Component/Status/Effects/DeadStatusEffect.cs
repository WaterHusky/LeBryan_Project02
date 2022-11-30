using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Core.Exceptions;
using Tactical.Battle.Controller;

namespace Tactical.Actor.Component {

	public class DeadStatusEffect : StatusEffect {

		private Unit owner;
		private Stats stats;

		private void Awake () {
			owner = GetComponentInParent<Unit>();
			stats = owner.GetComponent<Stats>();
		}

		private void OnEnable () {
			PlayEnableAnimation();

			this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
			this.AddObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats);
		}

		private void OnDisable () {
			PlayDisableAnimation();

			this.RemoveObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
			this.RemoveObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats);
		}

		private void OnTurnCheck (object sender, object args) {
			// Dont allow a KO'd unit to take turns
			var exc = args as BaseException;
			if (exc != null && exc.defaultToggle) {
				exc.FlipToggle();
			}
		}

		private void OnStatCounterWillChange (object sender, object args) {
			// Dont allow a KO'd unit to increment the turn order counter
			var exc = args as ValueChangeException;
			if (exc != null && exc.toValue > exc.fromValue) {
				exc.FlipToggle();
			}
		}

		private void PlayEnableAnimation () {
			owner.transform.localScale = new Vector3(0.75f, 0.1f, 0.75f);
		}

		private void PlayDisableAnimation () {
			owner.transform.localScale = Vector3.one;
		}

	}

}
