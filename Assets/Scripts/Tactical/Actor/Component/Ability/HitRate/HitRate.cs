using UnityEngine;
using Tactical.Core.Exceptions;
using Tactical.Actor.Model;
using Tactical.Grid.Component;

namespace Tactical.Actor.Component {

	/// <summary>
	/// The base class for Ability hit rate.
	/// </summary>
	public abstract class HitRate : MonoBehaviour {

		/// <summary>
		/// Includes a toggleable MatchException argument which defaults to false.
		/// </summary>
		public const string AutomaticHitCheckNotification = "HitRate.AutomaticHitCheckNotification";

		/// <summary>
		/// Includes a toggleable MatchException argument which defaults to false.
		/// </summary>
		public const string AutomaticMissCheckNotification = "HitRate.AutomaticMissCheckNotification";

		/// <summary>
		/// Includes an Info argument with three parameters: Attacker (Unit), Defender (Unit),
		/// and Defender's calculated Evade / Resistance (int). Status effects which modify Hit Rate
		/// should modify the arg2 parameter.
		/// </summary>
		public const string StatusCheckNotification = "HitRate.StatusCheckNotification";

		protected Unit attacker;

		protected virtual void Start () {
			attacker = GetComponentInParent<Unit>();
		}

		/// <summary>
		/// Returns a value in the range of 0 to 100 as a percent chance of
		/// an ability succeeding to hit.
		/// </summary>
		public abstract int Calculate (Tile target);

		public virtual bool RollForHit (Tile target) {
			int roll = Random.Range(0, 101);
			int chance = Calculate(target);
			return roll <= chance;
		}

		public virtual bool IsAngleBased {
			get { return true; }
		}

		protected virtual bool AutomaticHit (Unit target) {
			var exc = new MatchException(attacker, target);
			this.PostNotification(AutomaticHitCheckNotification, exc);
			return exc.toggle;
		}

		protected virtual bool AutomaticMiss (Unit target) {
			var exc = new MatchException(attacker, target);
			this.PostNotification(AutomaticMissCheckNotification, exc);
			return exc.toggle;
		}

		protected virtual int AdjustForStatusEffects (Unit target, int rate) {
			var args = new HitRateInfo(attacker, target, rate);
			this.PostNotification(StatusCheckNotification, args);
			return args.rate;
		}

		protected virtual int Final (int evade) {
			return 100 - evade;
		}

	}

}
