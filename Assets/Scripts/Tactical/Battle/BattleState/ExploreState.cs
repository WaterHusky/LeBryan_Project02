using Tactical.Grid.Model;
using Tactical.Core.Enums;
using Tactical.Core.EventArgs;

namespace Tactical.Battle.BattleState {

	public class ExploreState : BattleState {

		public override void Enter () {
			base.Enter();
			RefreshPrimaryStatPanel(pos);
		}

		public override void Exit () {
			base.Exit();
			statPanelController.HidePrimary();
		}

		protected override void OnMove (object sender, InfoEventArgs<Point> e) {
			SelectTile(e.info + pos);
			RefreshPrimaryStatPanel(pos);
		}

		protected override void OnAction (object sender, InfoEventArgs<BattleInputs> e) {
			base.OnAction(sender, e);

			if (e.info == BattleInputs.Confirm || e.info == BattleInputs.Cancel) {
				owner.ChangeState<CommandSelectionState>();
			}
		}

	}

}
