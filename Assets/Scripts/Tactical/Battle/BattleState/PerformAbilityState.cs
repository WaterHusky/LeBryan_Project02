using UnityEngine;
using System.Collections;
using Tactical.Actor.Component;

namespace Tactical.Battle.BattleState {

	public class PerformAbilityState : BattleState {

		public override void Enter () {
			base.Enter();
			turn.hasUnitActed = true;
			if (turn.hasUnitMoved) {
				turn.lockMove = true;
			}
			StartCoroutine(Animate());
		}

		private IEnumerator Animate () {
			yield return null;

			ApplyAbility();

			// The battle is over.
			if (IsBattleOver()) {
				owner.ChangeState<CutSceneState>();
			}
			// The unit finished their turn.
			else if (!UnitHasControl()) {
				owner.ChangeState<SelectUnitState>();
			}
			// The unit can still move.
			else if (turn.hasUnitMoved) {
				owner.ChangeState<EndFacingState>();
			}
			// The unit is starting their turn.
			else {
				owner.ChangeState<CommandSelectionState>();
			}
		}

		private bool UnitHasControl () {
			return turn.actor.GetComponentInChildren<DeadStatusEffect>() == null;
		}

		private void ApplyAbility () {
			Debug.Log(string.Format(
				"{0} using {1} on {2}.",
				turn.actor.name,
				turn.ability.name,
				turn.targets.Count == 0 ? "no target" : (turn.targets.Count == 1 ? turn.targets[0].content.name : string.Format("{0} targets", turn.targets.Count))
			), this);
			turn.ability.Perform(turn.targets, owner.audioSource);
		}

	}

}
