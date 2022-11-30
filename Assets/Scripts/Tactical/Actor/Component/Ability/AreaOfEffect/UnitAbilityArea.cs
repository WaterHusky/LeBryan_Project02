using UnityEngine;
using System.Collections.Generic;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// An AbilityArea to select an area of only one Unit.
	/// </summary>
	public class UnitAbilityArea : AbilityArea {

		public override List<Tile> GetTilesInArea (Board board, Point pos) {
			var retValue = new List<Tile>();
			Tile tile = board.GetTile(pos);

			if (tile != null) {
				retValue.Add(tile);
			}

			return retValue;
		}

	}

}
