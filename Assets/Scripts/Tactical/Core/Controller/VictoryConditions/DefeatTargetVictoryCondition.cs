using UnityEngine;
using Tactical.Core.Enums;

namespace Tactical.Core.Controller {

	public class DefeatTargetVictoryCondition : BaseVictoryCondition {

		public Tactical.Actor.Component.Unit target;

		protected override void CheckForGameOver () {
			base.CheckForGameOver();
			if (Victor == Alliances.None && IsDefeated(target)) {
				Victor = Alliances.Hero;
			}
		}

	}

}
