using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.Extensions;
using Tactical.Core.Controller;
using Tactical.Grid.Component;
using Tactical.Battle.Component;

namespace Tactical.Actor.Component {

	public class FreeMovement : MonoBehaviour {

		public float acceleration = 50f;
		public float maxSpeed = 5f;
		public float jumpForce = 200f;

		protected Unit unit;
		protected Rigidbody rb;

		protected virtual void Awake () {
			unit = GetComponent<Unit>();
			rb = GetComponent<Rigidbody>();
		}

		public void MoveHorizontally (Vector2 value) {
			rb.AddForce(new Vector3(value.x, 0f, value.y) * acceleration);

			// // TODO: enable this
			// // if (Mathf.Abs(rb.velocity.x) > maxSpeed) {
			// if (rb.velocity.magnitude >= maxSpeed) {
			// 	Debug.Log("Max speed: " + rb.velocity.magnitude);
			// 	rb.AddForce(rb.velocity * -1 * maxSpeed);
			// }

			rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
		}

		public void Jump () {
			var grounded = true;

			// TODO: move this in a Jump component
			// TODO: check if grounded
			if (grounded && rb.velocity.y <= 0.5f) {
				rb.AddForce(Vector3.up * jumpForce);
			}
		}

	}

}
