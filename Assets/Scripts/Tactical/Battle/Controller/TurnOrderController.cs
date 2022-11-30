using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.Exceptions;
using Tactical.Actor.Component;

namespace Tactical.Battle.Controller {

	public class TurnOrderController : MonoBehaviour {

		private const int turnActivation = 100;
		private const int turnCost = 50;
		private const int moveCost = 30;
		private const int actionCost = 20;

		public const string RoundBeganNotification = "TurnOrderController.roundBegan";
		public const string TurnBeganNotification = "TurnOrderController.TurnBeganNotification";
		public const string TurnCheckNotification = "TurnOrderController.turnCheck";
		public const string TurnCompletedNotification = "TurnOrderController.turnCompleted";
		public const string RoundEndedNotification = "TurnOrderController.roundEnded";

		public IEnumerator Round () {
			BattleController bc = GetComponent<BattleController>();
			while (true) {
				this.PostNotification(RoundBeganNotification);

				// Increase the CTR of each units with its SPD
				var units = new List<Unit>( bc.units );
				for (int i = 0; i < units.Count; ++i) {
					Stats s = units[i].GetComponent<Stats>();
					s[StatTypes.CTR] += s[StatTypes.SPD];
				}

				// Sort the units by CTR
				units.Sort( (a, b) => GetCounter(a).CompareTo(GetCounter(b)) );

				// Loop on units
				for (int i = units.Count - 1; i >= 0; --i) {
					if (CanTakeTurn(units[i])) {
						Stats s = units[i].GetComponent<Stats>();
						units[i].PostNotification(TurnBeganNotification, units);
						bc.turn.Change(units[i]);
						yield return units[i];

						// Calculate the cost of this turn.
						int cost = turnCost;
						if (bc.turn.hasUnitMoved) {
							cost += moveCost;
						}
						if (bc.turn.hasUnitActed) {
							cost += actionCost;
						}

						// And substract it to the CTR.

						s.SetValue(StatTypes.CTR, s[StatTypes.CTR] - cost, false);

						units[i].PostNotification(TurnCompletedNotification);
					}
				}

				this.PostNotification(RoundEndedNotification);
			}
		}

		private bool CanTakeTurn (Unit target) {
			var exc = new BaseException(GetCounter(target) >= turnActivation);
			target.PostNotification(TurnCheckNotification, exc);
			return exc.toggle;
		}

		private int GetCounter (Unit target) {
			return target.GetComponent<Stats>()[StatTypes.CTR];
		}
	}

}
