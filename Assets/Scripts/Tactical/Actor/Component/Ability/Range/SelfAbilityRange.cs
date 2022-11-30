using UnityEngine;
using System.Collections.Generic;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	public class SelfAbilityRange : AbilityRange {

		public override List<Tile> GetTilesInRange (Board board) {
			List<Tile> retValue = new List<Tile>(1);
			retValue.Add(unit.tile);
			return retValue;
		}

	}

}
