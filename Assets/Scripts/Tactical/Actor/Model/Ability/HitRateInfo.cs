using UnityEngine;
using Tactical.Actor.Component;

namespace Tactical.Actor.Model {

	public class HitRateInfo {

		public Unit attacker;
		public Unit target;
		public int rate;

		public HitRateInfo (Unit attacker, Unit target, int rate) {
			this.attacker = attacker;
			this.target = target;
			this.rate = rate;
		}
	}

}
