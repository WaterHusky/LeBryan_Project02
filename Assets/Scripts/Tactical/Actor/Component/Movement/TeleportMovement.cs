using UnityEngine;
using System.Collections;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	public class TeleportMovement : Movement {

		public override IEnumerator Traverse (Tile tile) {
			unit.Place(tile);

			Tweener spin = jumper.RotateToLocal(new Vector3(0, 360, 0), 0.5f, EasingEquations.EaseInOutQuad);
			spin.loopCount = 1;
			spin.loopType = EasingControl.LoopType.PingPong;
			var originalScale = transform.lossyScale;

			Tweener shrink = transform.ScaleTo(Vector3.zero, 0.5f, EasingEquations.EaseInBack);

			while (shrink != null) {
				yield return null;
			}

			transform.position = tile.center;

			Tweener grow = transform.ScaleTo(originalScale, 0.5f, EasingEquations.EaseOutBack);
			while (grow != null) {
				yield return null;
			}
		}

	}

}
