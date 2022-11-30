using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Tactical.UI.Component {

	/// <summary>
	/// A message that floats around and fades out.
	/// </summary>
	public class FloatingMessage : MonoBehaviour {

		public string message;
		public float duration = 1f;

		[Header("- Options -")]
		[SerializeField] private Color32 color = Color.white;
		[SerializeField] private int fontSize = 18;
		[SerializeField] private float speed = 2f;
		[Header("- Components -")]
		[SerializeField] private Text text;

		private void Update () {
			duration -= Time.deltaTime;
			if (duration <= 0f) {
				Destroy(gameObject);
			}

			if (!text) {
				Debug.LogError("Missing component: Text");
				return;
			}

			if (!string.IsNullOrEmpty(message)) {
				text.text = message;
			}

			text.color = color;
			text.fontSize = fontSize;

			transform.position = new Vector3(transform.position.x, transform.position.y + (speed * 50f * Time.deltaTime), transform.position.z);
		}

	}

}
