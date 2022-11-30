using UnityEngine;
using System.Collections.Generic;
using Tactical.Core.Exceptions;
using Tactical.Actor.Component;

namespace Tactical.Actor.Model {

	public class BaseAbilityEffectInfo {

		public Unit attacker;
		public Unit target;
		public List<ValueModifier> mods;

		public BaseAbilityEffectInfo (Unit attacker, Unit target, List<ValueModifier> mods) {
			this.attacker = attacker;
			this.target = target;
			this.mods = mods;
		}

	}

}
