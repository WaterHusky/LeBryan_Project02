using UnityEngine;
using Tactical.Actor.Component;

namespace Tactical.Actor.Model {

	/// <summary>
	/// An info to enable / disable a Status on a target.
	/// </summary>
	public class StatusInfo {

		/// <summary>
		/// The unit targetted by the check.
		/// </summary>
		public Unit target;

		/// <summary>
		/// Turn on to enable the Status.
		/// </summary>
		public bool enabled;

		public StatusInfo (Unit target) {
			this.target = target;
		}
	}

}
