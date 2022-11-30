using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Grid.Model;
using Tactical.Actor.Component;

namespace Tactical.AI.Component {

	public class PlanOfAttack {

		public Ability ability;
		public Targets target;
		public Point moveLocation;
		public Point fireLocation;
		public Directions attackDirection;

	}

}
