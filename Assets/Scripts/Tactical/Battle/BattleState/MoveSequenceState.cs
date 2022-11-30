using UnityEngine;
using System.Collections;
using Tactical.Actor.Component;

namespace Tactical.Battle.BattleState {

	public class MoveSequenceState : BattleState {

		public override void Enter () {
			base.Enter();
			StartCoroutine("Sequence");
		}

		private IEnumerator Sequence () {
			Movement m = turn.actor.GetComponent<Movement>();
			m.timeScale = owner.timeController.scale;
	    yield return StartCoroutine(m.Traverse(owner.currentTile));
	    turn.hasUnitMoved = true;
	    owner.ChangeState<CommandSelectionState>();
		}

	}

}

