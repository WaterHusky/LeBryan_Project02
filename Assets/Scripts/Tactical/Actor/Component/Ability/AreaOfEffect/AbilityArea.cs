using UnityEngine;
using System.Collections.Generic;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// The base class to set an Ability area.
	/// </summary>
	public abstract class AbilityArea : MonoBehaviour {

		/// <summary>
		/// Gets the tiles in the Ability area.
		/// </summary>
		///
		/// <param name="board">The board.</param>
		/// <param name="pos">The position.</param>
		///
		/// <returns>The tiles in the area.</returns>
		public abstract List<Tile> GetTilesInArea (Board board, Point pos);

	}

}
