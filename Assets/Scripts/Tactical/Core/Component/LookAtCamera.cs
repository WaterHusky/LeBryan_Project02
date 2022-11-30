using UnityEngine;
using UnityEngine.Assertions;

namespace Tactical.Core.Component {

	/// <summary>
	/// A basic component to make an object look at a camera.
	/// </summary>
	public class LookAtCamera : MonoBehaviour {

		/// <summary>
		/// The camera to look at.
		/// </summary>
		[SerializeField] private Camera targetCamera;

		private void Awake () {
			if (!targetCamera) {
				targetCamera = Camera.main;
			}

			Assert.IsNotNull(targetCamera);
		}

		private void Update () {
			LookAt(targetCamera.transform);
		}

		/// <summary>
		/// Changes the transform.rotation to look at the target.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		private void LookAt (Transform target) {
			transform.rotation = Quaternion.LookRotation(-target.forward);
		}

	}

}
