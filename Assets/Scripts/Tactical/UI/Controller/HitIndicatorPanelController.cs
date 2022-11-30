using UnityEngine;
using UnityEngine.UI;
using Tactical.UI.Component;

namespace Tactical.UI.Controller {

	public class HitIndicatorPanelController : MonoBehaviour {

		private const string ShowKey = "Show";
		private const string HideKey = "Hide";

		[SerializeField] private Canvas canvas;
		[SerializeField] private Panel panel;
		[SerializeField] private Image arrow;
		[SerializeField] private Text damageText;
		[SerializeField] private Text rateText;
		private Tweener transition;

		private void Start () {
			panel.SetPosition(HideKey, false);
		}

		public void SetStats (int rate, int amount) {
			arrow.fillAmount = (rate / 100f);
			string sign = (amount < 0) ? "" : "+";
			damageText.text = string.Format("{0}{1} {2}", sign, Mathf.Abs(amount), "HP");
			rateText.text = string.Format("{0}%", rate);
		}

		public void Show () {
			SetPanelPos(ShowKey);
		}

		public void Hide () {
			SetPanelPos(HideKey);
		}

		private void SetPanelPos (string pos) {
			if (transition != null && transition.IsPlaying) {
				transition.Stop();
			}

			transition = panel.SetPosition(pos, true);
			transition.duration = 0.5f;
			transition.equation = EasingEquations.EaseOutQuad;
		}

	}

}
