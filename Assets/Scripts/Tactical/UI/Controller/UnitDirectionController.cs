using UnityEngine;
using System;
using Tactical.Core.Enums;

namespace Tactical.UI.Controller {

	public class UnitDirectionController : MonoBehaviour {

		[SerializeField] private Renderer[] directions;
		[SerializeField] private Material materialDefault;
		[SerializeField] private Material materialActive;
		private Directions dir;

		private void Start () {
			Hide();

			if (directions.Length == 0) {
				throw new Exception(string.Format("Missing property: directions."));
			}
		}

		public void Show () {
			SetVisibility(true);
		}

		public void Show (Directions direction) {
			SetDirection(direction);
			Show();
		}

		public void Hide () {
			SetVisibility(false);
		}

		public void SetPosition (Vector3 position) {
			gameObject.transform.localPosition = position;
		}

		public void SetDirection (Directions dir) {
			int index = (int) dir;
			for (int i = 0; i < directions.Length; ++i) {
				directions[i].material = (i == index) ? materialActive : materialDefault;
			}

		}

		private void SetVisibility (bool visibility) {
			foreach (Renderer direction in directions) {
				direction.enabled = visibility;
			}
		}

	}

}
