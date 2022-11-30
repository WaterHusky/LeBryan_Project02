using UnityEngine;
using UnityEditor;
using Tactical.Actor.Component;

namespace Tactical.Actor.Editor {

	[CustomEditor(typeof(Health))]
	public class HealthInspector : UnityEditor.Editor {

		public Health current {
			get { return (Health) target; }
		}

		public override void OnInspectorGUI () {
			DrawDefaultInspector();

			GUILayout.Label(string.Format("HP: {0} / {1}", current.HP, current.MaxHP));
		}

	}

}
