using UnityEngine;

namespace Tactical.Core.Exceptions {

	public class MultiplyDeltaModifier : ValueModifier {

		public readonly float toMultiply;

		public MultiplyDeltaModifier (int sortOrder, float toMultiply) : base (sortOrder) {
			this.toMultiply = toMultiply;
		}

		public override float Modify (float fromValue, float toValue) {
			float delta = toValue - fromValue;
			return fromValue + delta * toMultiply;
		}
	}

}
