using UnityEngine;
using System.Collections.Generic;
using Tactical.Core.Exceptions;
using Tactical.Actor.Component;

namespace Tactical.Actor.Model {

	public class AbilityPowerInfo {

		public Unit attacker;
		public Unit target;
		public List<ValueModifier> mods;

		public AbilityPowerInfo (Unit attacker, Unit target, List<ValueModifier> mods) {
			this.attacker = attacker;
			this.target = target;
			this.mods = mods;
		}
	}

}
