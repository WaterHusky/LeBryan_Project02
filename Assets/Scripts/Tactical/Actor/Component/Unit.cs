using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Core.Extensions;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	public class Unit : MonoBehaviour {

		public Tile tile { get; protected set; }
		public Directions dir;

		public void Place (Tile target) {
			// Make sure old tile location is not still pointing to this unit
			if (tile != null && tile.content == gameObject) {
				tile.content = null;
			}

			// Link unit and tile references
			tile = target;

			if (target != null) {
				target.content = gameObject;
			}
		}

		public void Match () {
			transform.localPosition = tile.center;
			transform.localEulerAngles = dir.ToEuler();
		}

		//
		// Gizmos
		//

		private void OnDrawGizmos () {
			DrawAllianceBox();
		}

		private void DrawAllianceBox () {
			Color baseColor = GetGizmoBoxColor();
			Color transparentColor = baseColor;
			transparentColor.a = 0.2f;

			Gizmos.color = baseColor;
			Gizmos.DrawWireCube(transform.position, Vector3.one);

			Gizmos.color = transparentColor;
			Gizmos.DrawCube(transform.position, Vector3.one);
		}

		private Color GetGizmoBoxColor () {
			var alliance = GetComponent<Alliance>();
			var color = Color.grey;

			if (alliance != null) {
				switch (alliance.type) {
				case Alliances.Hero:
					color = Color.green;
					break;

				case Alliances.Enemy:
					color = Color.red;
					break;

				default:
					color = Color.grey;
					break;
				}
			}

			return color;
		}

	}

}
