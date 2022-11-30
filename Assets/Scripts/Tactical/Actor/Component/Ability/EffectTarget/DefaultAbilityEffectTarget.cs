using Tactical.Core.Enums;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	public class DefaultAbilityEffectTarget : AbilityEffectTarget {

		// TODO: exclude the unit doing the action.
		public override bool IsTarget (Tile tile) {
			if (tile == null || tile.content == null) {
				return false;
			}

			Stats s = tile.content.GetComponent<Stats>();
			return s != null && s[StatTypes.HP] > 0;
		}

	}

}
