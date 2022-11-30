using UnityEngine;
using UnityEngine.UI;

namespace Tactical.UI.Component {

	public class TurnOrderEntry : MonoBehaviour {

		[System.Flags]
		enum EntryState {
			None = 0,
			Selected = 1 << 0
		}

		public string Title {
			get { return label.text; }
			set { label.text = value; }
		}

		public bool IsSelected {
			get { return (State & EntryState.Selected) != EntryState.None; }
			set {
				if (value) {
					State |= EntryState.Selected;
				} else {
					State &= ~EntryState.Selected;
				}
			}
		}

		[SerializeField] private Text label;

		public void Reset () {
			State = EntryState.None;
		}

		private EntryState State {
			get { return state; }
			set {
				if (state == value) {
					return;
				}
				state = value;
			}
		}
		private EntryState state;

	}

}
