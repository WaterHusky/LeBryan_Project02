using UnityEngine;
using System.Collections.Generic;

namespace Tactical.AI.Component {

	public class AttackPattern : MonoBehaviour {

		public List<BaseAbilityPicker> pickers;
		private int index;

		public void Pick (PlanOfAttack plan) {
			pickers[index].Pick(plan);
			index++;
			if (index >= pickers.Count) {
				index = 0;
			}
		}

	}

}
