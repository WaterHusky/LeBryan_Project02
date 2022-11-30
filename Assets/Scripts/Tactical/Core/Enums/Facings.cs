using UnityEngine;

namespace Tactical.Core.Enums {

	/// <summary>
	/// An enum determining the direction of an attack.
	///
	/// B S S S F
	/// B B S F F
	/// B B > F F
	/// B B S F F
	/// B S S S F
	/// </summary>
	public enum Facings {
		Front,
		Side,
		Back
	}

}
