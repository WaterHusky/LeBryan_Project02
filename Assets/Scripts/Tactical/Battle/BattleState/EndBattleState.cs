using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tactical.Battle.BattleState {

	public class EndBattleState : BattleState {

		public override void Enter () {
			base.Enter();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

	}

}
