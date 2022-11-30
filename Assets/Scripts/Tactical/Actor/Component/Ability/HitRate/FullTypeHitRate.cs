using UnityEngine;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	public class FullTypeHitRate : HitRate {

		public override int Calculate (Tile target) {
			Unit defender = target.content.GetComponent<Unit>();
			if (AutomaticMiss(defender)) {
				return Final(100);
			}

			return Final (0);
		}

		public override bool IsAngleBased {
			get { return false; }
		}

	}

}
