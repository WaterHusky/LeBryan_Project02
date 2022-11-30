using UnityEngine;
using System;
using System.Collections;
using Tactical.Core.Model;
using Tactical.UI.Component;

namespace Tactical.Core.Controller {

	public class ConversationController : MonoBehaviour {

		public static event EventHandler completeEvent;

		[SerializeField] private ConversationPanel leftPanel;
		[SerializeField] private ConversationPanel rightPanel;
		private Canvas canvas;
		private IEnumerator conversation;
		private Tweener transition;
		private const string ShowTop = "Show Top";
		private const string ShowBottom = "Show Bottom";
		private const string HideTop = "Hide Top";
		private const string HideBottom = "Hide Bottom";

		private void Start () {
			canvas = GetComponentInChildren<Canvas>();
			if (leftPanel.panel.CurrentPosition == null) {
				leftPanel.panel.SetPosition(HideBottom, false);
			}
			if (rightPanel.panel.CurrentPosition == null) {
				rightPanel.panel.SetPosition(HideBottom, false);
			}
			canvas.gameObject.SetActive(false);
		}

		public void Show (ConversationData data) {
			canvas.gameObject.SetActive(true);
			conversation = Sequence(data);
			conversation.MoveNext();
		}

		public void Next () {
			if (conversation == null || transition != null) {
				return;
			}

			conversation.MoveNext();
		}

		private IEnumerator Sequence (ConversationData data) {
			for (int i = 0; i < data.list.Count; ++i) {
				SpeakerData sd = data.list[i];

				ConversationPanel currentPanel = (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.MiddleLeft || sd.anchor == TextAnchor.LowerLeft) ? leftPanel : rightPanel;
				IEnumerator presenter = currentPanel.Display(sd);
				presenter.MoveNext();

				string show, hide;
				if (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.UpperCenter || sd.anchor == TextAnchor.UpperRight) {
					show = ShowTop;
					hide = HideTop;
				} else {
					show = ShowBottom;
					hide = HideBottom;
				}

				currentPanel.panel.SetPosition(hide, false);
				MovePanel(currentPanel, show);

				yield return null;
				while (presenter.MoveNext()) {
					yield return null;
				}

				MovePanel(currentPanel, hide);
				transition.completedEvent += delegate(object sender, System.EventArgs e) {
					conversation.MoveNext();
				};

				yield return null;
			}

			canvas.gameObject.SetActive(false);
			if (completeEvent != null) {
				completeEvent(this, System.EventArgs.Empty);
			}
		}

		private void MovePanel (ConversationPanel obj, string pos) {
			transition = obj.panel.SetPosition(pos, true);
			transition.duration = 0.5f;
			transition.equation = EasingEquations.EaseOutQuad;
		}

	}

}
