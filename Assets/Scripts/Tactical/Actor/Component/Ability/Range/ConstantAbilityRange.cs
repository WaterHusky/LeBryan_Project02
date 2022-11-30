using UnityEngine;
using System.Collections.Generic;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	public class ConstantAbilityRange : AbilityRange {

		public override List<Tile> GetTilesInRange (Board board) {
			return board.Search(unit.tile, ExpandSearch);
		}

		private bool ExpandSearch (Tile from, Tile to) {
			return (from.distance + 1) <= horizontal && Mathf.Abs(to.height - unit.tile.height) <= vertical;
		}
	}

}
