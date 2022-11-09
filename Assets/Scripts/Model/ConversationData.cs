using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConversationData : ScriptableObject 
{
	[NonReorderable]
	public List<SpeakerData> list;
}
