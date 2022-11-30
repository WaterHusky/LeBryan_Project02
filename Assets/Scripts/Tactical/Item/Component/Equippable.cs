using UnityEngine;
using Tactical.Core;
using Tactical.Core.Enums;
using Tactical.Core.Component;

namespace Tactical.Item.Component {

	public class Equippable : MonoBehaviour {

		public EquipSlots defaultSlots;
		public EquipSlots secondarySlots;
		public EquipSlots slots;
		private bool _isEquipped;

		public void OnEquip () {
			if (_isEquipped) {
				return;
			}

			_isEquipped = true;

			Feature[] features = GetComponentsInChildren<Feature>();
			for (int i = 0; i < features.Length; ++i) {
				features[i].Activate(gameObject);
			}
		}

		public void OnUnEquip () {
			if (!_isEquipped) {
				return;
			}

			_isEquipped = false;

			Feature[] features = GetComponentsInChildren<Feature>();
			for (int i = 0; i < features.Length; ++i) {
				features[i].Deactivate();
			}
		}

	}

}
