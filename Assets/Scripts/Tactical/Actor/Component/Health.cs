using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Core.Exceptions;

namespace Tactical.Actor.Component {

	public class Health : MonoBehaviour {

		public int HP {
			get { return stats[StatTypes.HP]; }
			set { stats[StatTypes.HP] = value; }
		}

		public int MaxHP {
			get { return stats[StatTypes.MHP]; }
			set { stats[StatTypes.MHP] = value; }
		}

		public int MinHP;

		private Stats stats;

		private void Awake () {
			stats = GetComponent<Stats>();
		}

		private void OnEnable () {
			this.AddObserver(OnHPWillChange, Stats.WillChangeNotification(StatTypes.HP), stats);
			this.AddObserver(OnMaxPDidChange, Stats.DidChangeNotification(StatTypes.MHP), stats);
		}

		private void OnDisable () {
			this.RemoveObserver(OnHPWillChange, Stats.WillChangeNotification(StatTypes.HP), stats);
			this.RemoveObserver(OnMaxPDidChange, Stats.DidChangeNotification(StatTypes.MHP), stats);
		}

		private void OnHPWillChange (object sender, object args) {
			var vce = args as ValueChangeException;
			vce.AddModifier(new ClampValueModifier(int.MaxValue, MinHP, stats[StatTypes.MHP]));
		}

		private void OnMaxPDidChange (object sender, object args) {
			int oldMaxHP = (int) args;
			if (MaxHP > oldMaxHP) {
				HP += MaxHP - oldMaxHP;
			} else {
				HP = Mathf.Clamp(HP, MinHP, MaxHP);
			}
		}

	}

}
