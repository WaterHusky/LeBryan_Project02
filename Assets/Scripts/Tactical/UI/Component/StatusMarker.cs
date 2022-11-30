using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Tactical.UI.Component {

	public class StatusMarker : MonoBehaviour {

		public Image image;
		public Text title;
		public Text description;
		public GameObject content;
		// [HideInInspector] public StatusIcon icon;

		private void OnValidate () {
			Assert.IsNotNull(image);
			Assert.IsNotNull(title);
			Assert.IsNotNull(description);
			Assert.IsNotNull(content);
		}

		private void Update () {
			// Assert.IsNotNull(icon);

			// if (!string.IsNullOrEmpty(icon.name)) {
			// 	title.text = icon.name;
			// }

			// if (!string.IsNullOrEmpty(icon.name)) {
			// 	name = icon.title;
			// }

			// if (!string.IsNullOrEmpty(icon.description)) {
			// 	description.text = icon.description;
			// }

			// var sprite = Resources.Load<Sprite>("UI/Icons/" + icon.name);
			// image.sprite = sprite;
		}

		public void ShowTooltip () {
			Debug.Log("ShowTooltip");
			content.SetActive(true);
		}

		public void HideTooltip () {
			Debug.Log("HideTooltip");
			content.SetActive(false);
		}

	}

}
