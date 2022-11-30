using UnityEngine;
using UnityEngine.Assertions;

namespace Tactical.Core.Component {

	public class StateTrigger : MonoBehaviour {

		public const string StateChangeTriggered = "StateTrigger.StateChangeTriggered";

		public string state;
		public string[] tags = { "Player" };

		public void OnValidate () {
			Assert.IsNotNull(state, "state required");
			Assert.IsTrue(tags.Length > 0, "tags required");
		}

		private void OnTriggerEnter (Collider other) {
			foreach (string t in tags) {
				if (other && other.tag == t) {
					this.PostNotification(StateChangeTriggered, state);
				}
			}
		}

	}

}
