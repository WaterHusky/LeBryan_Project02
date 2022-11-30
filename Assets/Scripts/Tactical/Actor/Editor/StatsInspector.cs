using UnityEngine;
using UnityEditor;
using Tactical.Core.Enums;
using Tactical.Actor.Component;

namespace Tactical.Actor.Editor {

	[CustomEditor(typeof(Stats))]
	public class StatsInspector : UnityEditor.Editor {

		public Stats current {
			get { return (Stats) target; }
		}

		public override void OnInspectorGUI () {
			DrawDefaultInspector();

			GUILayout.Label(string.Format("Level: {0}", current[StatTypes.LVL]));
			BasicStatButton(StatTypes.LVL, 1);

			GUILayout.Label(string.Format("Experience: {0}", current[StatTypes.EXP]));
			BasicStatButton(StatTypes.EXP, 25);
		}

		private void BasicStatButton (StatTypes statType, int step) {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("-" + step)) {
				current.SetValue(statType, current[statType] - step, true);
			}
			if (GUILayout.Button("+" + step)) {
				current.SetValue(statType, current[statType] + step, true);
			}
			GUILayout.EndHorizontal();
		}

	}

}
