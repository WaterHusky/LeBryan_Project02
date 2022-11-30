using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Tactical.UI.Component;

namespace Tactical.UI.Controller {

	public class ActionMenuPanelController : MonoBehaviour {

		public int selection { get; private set; }

		[SerializeField] private GameObject entryPrefab;
		[SerializeField] private Text titleLabel;
		[SerializeField] private Panel panel;
		[SerializeField] private GameObject canvas;
		[SerializeField] private AudioClip selectionClip;
		[SerializeField] private AudioSource audioSource;
		private List<ActionMenuEntry> menuEntries = new List<ActionMenuEntry>(MenuCount);
		private const string ShowKey = "Show";
		private const string HideKey = "Hide";
		private const string EntryPoolKey = "ActionMenuPanel.Entry";
		private const int MenuCount = 4;

		private void Awake () {
			GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
		}

		private void Start () {
			panel.SetPosition(HideKey, false);
			canvas.SetActive(false);
		}

		public void Show (string title, List<string> options) {
			canvas.SetActive(true);
			Clear();
			titleLabel.text = title;
			for (int i = 0; i < options.Count; ++i) {
				ActionMenuEntry entry = Dequeue();
				entry.Title = options[i];
				menuEntries.Add(entry);
			}
			SetSelection(0);
			TogglePos(ShowKey);
		}

		public void Hide () {
			Tweener t = TogglePos(HideKey);
			t.completedEvent += delegate(object sender, System.EventArgs e) {
				if (panel.CurrentPosition == panel[HideKey]) {
					Clear();
					canvas.SetActive(false);
				}
			};
		}

		public void SetLocked (int index, bool value) {
			if (index < 0 || index >= menuEntries.Count) {
				return;
			}

			menuEntries[index].IsLocked = value;
			if (value && selection == index) {
				Next();
			}
		}

		public void Next () {
			for (int i = selection + 1; i < selection + menuEntries.Count; ++i) {
				int index = i % menuEntries.Count;
				if (SetSelection(index)) {
					break;
				}
			}
		}

		public void Previous () {
			for (int i = selection - 1 + menuEntries.Count; i > selection; --i) {
				int index = i % menuEntries.Count;
				if (SetSelection(index)) {
					break;
				}
			}
		}

		private ActionMenuEntry Dequeue () {
			Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
			ActionMenuEntry entry = p.GetComponent<ActionMenuEntry>();
			entry.transform.SetParent(panel.transform, false);
			entry.transform.localScale = Vector3.one;
			entry.gameObject.SetActive(true);
			entry.Reset();
			return entry;
		}

		private void Enqueue (ActionMenuEntry entry) {
			Poolable p = entry.GetComponent<Poolable>();
			GameObjectPoolController.Enqueue(p);
		}

		private void Clear () {
			for (int i = menuEntries.Count - 1; i >= 0; --i) {
				Enqueue(menuEntries[i]);
			}
			menuEntries.Clear();
		}

		private Tweener TogglePos (string pos) {
			Tweener t = panel.SetPosition(pos, true);
			t.duration = 0.5f;
			t.equation = EasingEquations.EaseOutQuad;
			return t;
		}

		private bool SetSelection (int value) {
			if (menuEntries[value].IsLocked) {
				return false;
			}

			// Deselect the previously selected entry
			if (selection >= 0 && selection < menuEntries.Count) {
				menuEntries[selection].IsSelected = false;
			}

			selection = value;

			// Select the new entry
			if (selection >= 0 && selection < menuEntries.Count) {
				menuEntries[selection].IsSelected = true;
			}

			// Play the selection clip.
			if (audioSource && selectionClip) {
				audioSource.clip = selectionClip;
				audioSource.Play();
			}

			return true;
		}

	}

}
