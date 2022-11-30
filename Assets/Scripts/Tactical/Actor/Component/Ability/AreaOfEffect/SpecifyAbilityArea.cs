using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// An AbilityArea to select an area of a specific range.
	/// </summary>
	public class SpecifyAbilityArea : AbilityArea {

		/// <summary>
		/// The number of tiles the Ability can traverse horizontally.
		/// </summary>
		public int horizontal;
		/// <summary>
		/// The number of tiles the Ability can traverse vertically.
		/// </summary>
		public int vertical;

		/// <summary>
		/// The base Tile used to get the other tiles.
		/// </summary>
		private Tile tile;

		private void OnValidate () {
			Assert.IsTrue(horizontal > 0, "Value must be greater than 0: horizontal.");
		}

		public override List<Tile> GetTilesInArea (Board board, Point pos) {
			tile = board.GetTile(pos);
			return board.Search(tile, ExpandSearch);
		}

		/// <summary>
		/// Matchs only tiles within the horizontal / vertical range.
		/// </summary>
		///
		/// <param name="from">The from Tile.</param>
		/// <param name="to">The to Tile.</param>
		///
		/// <returns>True if the Tile is within the range, False otherwise.</returns>
		private bool ExpandSearch (Tile from, Tile to) {
			return (
				(from.distance + 1) <= horizontal &&
				Mathf.Abs(to.height - tile.height) <= vertical
			);
		}

	}

}
