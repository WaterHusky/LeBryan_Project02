using UnityEngine;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// The base class to determine an AbilityEffect target.
	/// </summary>
	public abstract class AbilityEffectTarget : MonoBehaviour {

		/// <summary>
		/// Determines if a tile is a valid target.
		/// </summary>
		///
		/// <param name="tile">The tile.</param>
		///
		/// <returns>True if valid target, False otherwise.</returns>
		public abstract bool IsTarget (Tile tile);

	}

}
