using UnityEngine;
using System;
using System.Collections.Generic;
using Tactical.Actor.Component;

using Party = System.Collections.Generic.List<UnityEngine.GameObject>;

namespace Tactical.Actor.Controller {

	public static class ExperienceController {

		private const float minLevelBonus = 1.5f;
		private const float maxLevelBonus = 0.5f;

		public static void AwardExperience (int amount, Party party) {
			// Grab a list of all of the rank components from our hero party
			var ranks = new List<ExperienceRank>(party.Count);
			for (int i = 0; i < party.Count; ++i) {
				ExperienceRank r = party[i].GetComponent<ExperienceRank>();
				if (r != null) {
					ranks.Add(r);
				}
			}

			// Step 1: determine the range in actor level stats
			int min = int.MaxValue;
			int max = int.MinValue;
			for (int i = ranks.Count - 1; i >= 0; --i) {
				min = Mathf.Min(ranks[i].Level, min);
				max = Mathf.Max(ranks[i].Level, max);
			}

			// Step 2: weight the amount to award per actor based on their level
			var weights = new float[party.Count];
			float summedWeights = 0;
			for (int i = ranks.Count - 1; i >= 0; --i) {
				float percent = (float)(ranks[i].Level - min) / (float)(max - min);
				weights[i] = Mathf.Lerp(minLevelBonus, maxLevelBonus, percent);
				summedWeights += weights[i];
			}

			// Step 3: hand out the weighted award
			for (int i = ranks.Count - 1; i >= 0; --i) {
				int subAmount = Mathf.FloorToInt((weights[i] / summedWeights) * amount);
				ranks[i].Experience += subAmount;
			}

		}

	}

}
