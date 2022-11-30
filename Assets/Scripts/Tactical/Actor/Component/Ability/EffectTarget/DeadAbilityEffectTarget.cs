using Tactical.Core.Enums;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	public class DeadAbilityEffectTarget : AbilityEffectTarget {

		public override bool IsTarget (Tile tile) {
			if (tile == null || tile.content == null) {
				return false;
			}

			Stats s = tile.content.GetComponent<Stats>();
			Health health = tile.content.GetComponent<Health>();
			return s != null && s[StatTypes.HP] <= health.MinHP;
		}

	}

}
