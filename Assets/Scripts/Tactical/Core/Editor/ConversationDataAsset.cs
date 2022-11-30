using UnityEditor;
using Tactical.Core.Model;

namespace Tactical.Core.Editor {

	public static class ConversationDataAsset {

		[MenuItem("Assets/Create/Conversation Data")]
		public static void CreateConversationData () {
			ScriptableObjectUtility.CreateAsset<ConversationData>();
		}

	}

}
