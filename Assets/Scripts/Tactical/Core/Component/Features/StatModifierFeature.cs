using Tactical.Core.Enums;
using Tactical.Actor.Component;

namespace Tactical.Core.Component {

	public class StatModifierFeature : Feature {

		public StatTypes type;
		public int amount;

		private Stats stats {
			get {
				return _target.GetComponentInParent<Stats>();
			}
		}

		protected override void OnApply () {
			stats[type] += amount;
		}

		protected override void OnRemove () {
			stats[type] -= amount;
		}

	}

}
