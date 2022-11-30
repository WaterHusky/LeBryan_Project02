using UnityEngine;

namespace Tactical.Battle.Component {

	public class CameraRig : MonoBehaviour {

		public float speed = 3f;
		public float rotateSpeed = 300f;
		public Transform follow;
		[HideInInspector] public Vector3 rotateTowards;

		private Transform _transform;

		private void Awake () {
			_transform = transform;
		}

		private void Update () {
			if (follow) {
				_transform.position = Vector3.Lerp(_transform.position, follow.position, speed * Time.deltaTime);
			}

			if (rotateTowards != transform.rotation.eulerAngles) {
        float step = rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(rotateTowards), step);
			}
		}
	}

}
