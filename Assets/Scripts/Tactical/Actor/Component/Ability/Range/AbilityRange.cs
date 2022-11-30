using UnityEngine;
using System.Collections.Generic;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	public abstract class AbilityRange : MonoBehaviour {

		public int horizontal = 1;
		public int vertical = int.MaxValue;
		public virtual bool directionOriented {
			get { return false; }
		}
		public virtual bool positionOriented {
			get { return true; }
		}

		protected Unit unit {
			get { return GetComponentInParent<Unit>(); }
		}

		public abstract List<Tile> GetTilesInRange (Board board);

	}

}
