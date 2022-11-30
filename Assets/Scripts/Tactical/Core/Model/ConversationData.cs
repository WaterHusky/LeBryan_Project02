using UnityEngine;
using System.Collections.Generic;

namespace Tactical.Core.Model {

	public class ConversationData : ScriptableObject {

		[NonReorderable]
		public List<SpeakerData> list;

	}

}
