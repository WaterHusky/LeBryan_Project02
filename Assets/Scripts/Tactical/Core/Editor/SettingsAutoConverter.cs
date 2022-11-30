using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;

namespace Tactical.Core.Editor {

	public class SettingsAutoConverter : AssetPostprocessor {

		private readonly static Dictionary<string, Action> parsers;

		static SettingsAutoConverter () {
			parsers = new Dictionary<string, Action>();
			parsers.Add("JobStartingStats.csv", Tactical.Actor.Editor.JobParser.Parse);
			parsers.Add("JobGrowthStats.csv", Tactical.Actor.Editor.JobParser.Parse);
			parsers.Add("Enemies.csv", EnemyParser.Parse);
		}

		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
			for (int i = 0; i < importedAssets.Length; i++) {
				string fileName = Path.GetFileName( importedAssets[i] );
				if (parsers.ContainsKey(fileName)) {
					parsers[fileName]();
				}
			}
			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh();
		}
	}

}
