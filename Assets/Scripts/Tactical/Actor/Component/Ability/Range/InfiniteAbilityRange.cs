using UnityEngine;
using System.Collections.Generic;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	public class InfiniteAbilityRange : AbilityRange {

		public override List<Tile> GetTilesInRange (Board board) {
			return new List<Tile>(board.tiles.Values);
		}

		public override bool positionOriented {
			get { return false; }
		}

	}

}
