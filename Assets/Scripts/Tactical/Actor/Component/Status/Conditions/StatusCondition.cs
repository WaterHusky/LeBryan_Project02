using UnityEngine;
using System;

namespace Tactical.Actor.Component {

	public class StatusCondition : MonoBehaviour {

		public virtual void Remove () {
			Status s = GetComponentInParent<Status>();
			if (s) {
				s.Remove(this);
			}
		}

	}

}
