using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Reflection;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// An BaseAbilityEffect to inclict a Status effect to a target.
	/// </summary>
	public class InflictAbilityEffect : BaseAbilityEffect {

		/// <summary>
		/// The name of the status to apply.
		/// </summary>
		public string statusName;

		/// <summary>
		/// The duration (in turns) of the effect.
		/// </summary>
		public int duration;

		private Type statusType;

		/// <summary>
		/// The namespace used for all Status.
		/// </summary>
		private const string statusTypeNamespace = "Tactical.Actor.Component";

		public override int Predict (Tile target) {
			return 0;
		}

		protected override void Awake () {
			base.Awake();

			statusType = Type.GetType(string.Format("{0}.{1}", statusTypeNamespace, statusName));

			Assert.IsNotNull(statusType);
			Assert.IsTrue(statusType.IsSubclassOf(typeof(StatusEffect)));
		}

		private void OnValidate () {
			Assert.IsNotNull(statusName);
			Assert.IsTrue(duration > 0);
		}

		/// <summary>
		/// Applies the Status effect and update the condition duration.
		/// </summary>
		///
		/// <param name="target">The target.</param>
		///
		/// <returns>Always returns 0 since it doesn't do damage by itself.</returns>
		protected override int OnApply (Tile target) {
			MethodInfo methodInfo = typeof(Status).GetMethod("Add");
			Type[] types = { statusType, typeof(DurationStatusCondition) };
			MethodInfo constructed = methodInfo.MakeGenericMethod(types);

			var status = target.content.GetComponent<Status>();
			object retValue = constructed.Invoke(status, null);

			var condition = retValue as DurationStatusCondition;
			if (condition != null) {
				condition.duration = duration;
			}

			return 0;
		}

	}

}
