using UnityEngine;
using Tactical.Core.Enums;

namespace Tactical.Actor.Component {

	public class Driver : MonoBehaviour {

		public Drivers normal;
		public Drivers special;

		public Drivers Current {
			get {
				return special != Drivers.None ? special : normal;
			}
		}

	}

}
