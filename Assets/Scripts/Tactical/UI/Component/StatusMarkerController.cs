using UnityEngine;
using System.Collections.Generic;

namespace Tactical.UI.Component {

	public class StatusMarkerController : MonoBehaviour {

		private const string prefabPath = "UI/Status Markers/";

		/// <summary>
		/// The markers available.
		///
		/// @todo Load this from some CSV file.
		/// </summary>
		private Dictionary<string, string> markers = new Dictionary<string, string>();

		private void Awake () {
			markers.Add("SilenceStatusEffect", "Silence");
			markers.Add("BlindStatusEffect", "Blind");
		}

		/// <summary>
		/// Adds a marker the the list.
		/// </summary>
		///
		/// <param name="statusName">The name of the status.</param>
		public void Add (string statusName) {
			CreateMarker(statusName);
		}

		/// <summary>
		/// Removes a marker from the list.
		/// </summary>
		///
		/// <param name="statusName">The name of the status.</param>
		public void Remove (string statusName) {
			DestroyMarker(statusName);
 		}

 		/// <summary>
 		/// Creates the actual GameObject as a child with the correct options.
 		/// </summary>
 		///
 		/// <param name="statusName">The marker name.</param>
		private void CreateMarker (string statusName) {
			if (!markers.ContainsKey(statusName)) {
				Debug.LogWarning(string.Format("Missing marker for status: {0}", statusName));
				return;
			}

			var marker = markers[statusName];
			GameObject prefab = Resources.Load<GameObject>(prefabPath + marker);
			GameObject instance = Instantiate(prefab);
			instance.name = instance.name.Replace("(Clone)", "");
			instance.transform.SetParent(transform, false);
		}

		/// <summary>
		/// Destroys a child GameObject using the status name.
		/// </summary>
		///
		/// <param name="statusName">The status name.</param>
		private void DestroyMarker (string statusName) {
			var status = transform.Find(statusName);
			if (status) {
				Destroy(status.gameObject);
			}
		}

	}

}
