using UnityEngine;
using Tactical.Core.Enums;
using Tactical.Core.Exceptions;

namespace Tactical.Actor.Component {

	public class ExperienceRank : MonoBehaviour {

		public const int MIN_LEVEL = 1;
		public const int MAX_LEVEL = 30;
		public const int MAX_EXPERIENCE = 999999;
		public const int EXPERIENCE_PER_LEVEL = 100;

		public int Level {
			get { return stats[StatTypes.LVL]; }
		}

		public int Experience {
			get { return stats[StatTypes.EXP]; }
			set { stats[StatTypes.EXP] = value; }
		}

		public float LevelPercent {
			get { return (float)(Level - MIN_LEVEL) / (float)(MAX_LEVEL - MIN_LEVEL); }
		}

		private Stats stats;

		private void Awake () {
			stats = GetComponent<Stats>();
		}

		private void OnEnable () {
			this.AddObserver(OnExpWillChange, Stats.WillChangeNotification(StatTypes.EXP), stats);
			this.AddObserver(OnExpDidChange, Stats.DidChangeNotification(StatTypes.EXP), stats);
		}

		private void OnDisable () {
			this.RemoveObserver(OnExpWillChange, Stats.WillChangeNotification(StatTypes.EXP), stats);
			this.RemoveObserver(OnExpDidChange, Stats.DidChangeNotification(StatTypes.EXP), stats);
		}

		private void OnExpWillChange (object sender, object args) {
			var vce = args as ValueChangeException;
			if (vce != null) {
				vce.AddModifier(new ClampValueModifier(int.MaxValue, Experience, MAX_EXPERIENCE));
			}
		}

		private void OnExpDidChange (object sender, object args) {
			stats.SetValue(StatTypes.LVL, LevelForExperience(Experience), false);
		}

		public static int ExperienceForLevel (int level) {
			return Mathf.Min(level, MAX_LEVEL) * EXPERIENCE_PER_LEVEL;
		}

		public static int LevelForExperience (int exp) {
			int lvl = MAX_LEVEL;
			for (; lvl >= MIN_LEVEL; --lvl)
				if (exp >= ExperienceForLevel(lvl)) {
					break;
				}
			return lvl;
		}

		public void Init (int level) {
			stats.SetValue(StatTypes.LVL, level, false);
			stats.SetValue(StatTypes.EXP, ExperienceForLevel(level), false);
		}

	}

}
