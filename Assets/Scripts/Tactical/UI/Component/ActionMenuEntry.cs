using UnityEngine;
using UnityEngine.UI;

namespace Tactical.UI.Component {

	public class ActionMenuEntry : MonoBehaviour {

		[System.Flags]
		enum EntryState {
			None = 0,
			Selected = 1 << 0,
			Locked = 1 << 1
		}

		public string Title {
			get { return label.text; }
			set { label.text = value; }
		}
		public bool IsLocked {
			get { return (State & EntryState.Locked) != EntryState.None; }
			set {
				if (value) {
					State |= EntryState.Locked;
				} else {
					State &= ~EntryState.Locked;
				}
			}
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

		[SerializeField] private Image bullet;
		[SerializeField] private Color normalColor = Color.white;
		[SerializeField] private Color selectedColor = new Color32(217, 172, 51, 255);
		[SerializeField] private Color disabledColor = new Color32(150, 150, 150, 255);
		[SerializeField] private Text label;
		private Outline outline;
		private Image bulletImage;

		private void Awake () {
			outline = label.GetComponent<Outline>();
			bulletImage = bullet.GetComponent<Image>();
		}

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

				if (IsLocked) {
					bulletImage.color = disabledColor;
					label.color = disabledColor;
				} else if (IsSelected) {
					bulletImage.color = selectedColor;
					label.color = selectedColor;
				} else {
					bulletImage.color = normalColor;
					label.color = normalColor;
				}
			}
		}
		private EntryState state;

	}

}
