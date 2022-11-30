using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Tactical.Core;
using Tactical.UI.Component;
using Tactical.Actor.Component;
using Tactical.Battle.Controller;

namespace Tactical.UI.Controller {

	public class TurnOrderPanelController : MonoBehaviour {

		public int current { get; private set; }

		[SerializeField] private GameObject entryPrefab;
		[SerializeField] private Text titleLabel;
		[SerializeField] private Panel panel;
		[SerializeField] private GameObject canvas;
		private const string ShowKey = "Show";
		private const string HideKey = "Hide";
		private const string EntryPoolKey = "TurnOrderPanel.Entry";
		private const int ActorCount = 10;
		private List<TurnOrderEntry> turnOrderEntries = new List<TurnOrderEntry>(ActorCount);

		private void OnEnable () {
			this.AddObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification);
		}

		private void OnDisable () {
			this.RemoveObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification);
		}

		private void Awake () {
			GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, ActorCount, int.MaxValue);
		}

		private void Start () {
			panel.SetPosition(HideKey, false);
			canvas.SetActive(false);
		}

		public void Show () {
			canvas.SetActive(true);
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

		public void Next () {
			for (int i = current + 1; i < current + turnOrderEntries.Count; ++i) {
				int index = i % turnOrderEntries.Count;
				if (SetCurrent(index)) {
					break;
				}
			}
		}

		public void Previous () {
			for (int i = current - 1 + turnOrderEntries.Count; i > current; --i) {
				int index = i % turnOrderEntries.Count;
				if (SetCurrent(index)) {
					break;
				}
			}
		}

		private TurnOrderEntry Dequeue () {
			Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
			TurnOrderEntry entry = p.GetComponent<TurnOrderEntry>();
			entry.transform.SetParent(panel.transform, false);
			entry.transform.localScale = Vector3.one;
			entry.gameObject.SetActive(true);
			entry.Reset();
			return entry;
		}

		private void Enqueue (TurnOrderEntry entry) {
			Poolable p = entry.GetComponent<Poolable>();
			GameObjectPoolController.Enqueue(p);
		}

		private void UpdateEntries (List<Unit> units) {
			Clear();
			for (int i = units.Count - 1; i >= 0; --i) {
				TurnOrderEntry entry = Dequeue();
				entry.Title = units[i].name;
				turnOrderEntries.Add(entry);
			}
		}

		private void Clear () {
			for (int i = turnOrderEntries.Count - 1; i >= 0; --i) {
				Enqueue(turnOrderEntries[i]);
			}
			turnOrderEntries.Clear();
		}

		private Tweener TogglePos (string pos) {
			Tweener t = panel.SetPosition(pos, true);
			t.duration = 0.5f;
			t.equation = EasingEquations.EaseOutQuad;
			return t;
		}

		private bool SetCurrent (int value) {
			current = value;

			// Select the new entry
			if (current >= 0 && current < turnOrderEntries.Count) {
				turnOrderEntries[current].IsSelected = true;
			}

			return true;
		}

		private void OnTurnBegan (object sender, object args) {
			var units = args as List<Unit>;
			UpdateEntries(units);
		}

	}

}
