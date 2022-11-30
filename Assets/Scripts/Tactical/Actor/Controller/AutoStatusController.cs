using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Actor.Component;

namespace Tactical.Actor.Controller {

	public class AutoStatusController : MonoBehaviour {

		private void OnEnable () {
			this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
		}

		private void OnDisable () {
			this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
		}

		private void OnHPDidChangeNotification (object sender, object args) {
			var stats = sender as Stats;
			if (stats[StatTypes.HP] <= 0) {
				Status status = stats.GetComponentInChildren<Status>();
				StatComparisonCondition c = status.Add<DeadStatusEffect, StatComparisonCondition>();
				c.Init(StatTypes.HP, 0, c.EqualTo);
			}
		}

	}

}
