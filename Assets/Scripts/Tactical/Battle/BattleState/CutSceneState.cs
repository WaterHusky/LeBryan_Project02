using UnityEngine;
using System.Collections;
using Tactical.Core.Enums;
using Tactical.Core.EventArgs;
using Tactical.Core.Controller;
using Tactical.Core.Model;

namespace Tactical.Battle.BattleState {

	public class CutSceneState : BattleState {

		private const string introScene = "IntroScene";
		private const string outroWinScene = "OutroSceneWin";
		private const string outroLoseScene = "OutroSceneLose";

		private ConversationController conversationController;
		private ConversationData data;

		protected override void Awake () {
			base.Awake();
			conversationController = owner.GetComponentInChildren<ConversationController>();
		}

		public override void Enter () {
			base.Enter();
			if (IsBattleOver()) {
				if (DidPlayerWin()) {
					data = Resources.Load<ConversationData>("Conversations/" + outroWinScene);
				} else {
					data = Resources.Load<ConversationData>("Conversations/" + outroLoseScene);
				}
			} else {
				data = Resources.Load<ConversationData>("Conversations/" + introScene);
			}

			if (data == null) {
				StartCoroutine("NextStateCoroutine");
				return;
			}

			conversationController.Show(data);
		}

		public override void Exit () {
			base.Exit();
			if (data) {
				Resources.UnloadAsset(data);
			}
		}

		protected override void AddListeners () {
			base.AddListeners();
			ConversationController.completeEvent += OnCompleteConversation;
		}

		protected override void RemoveListeners () {
			base.RemoveListeners();
			ConversationController.completeEvent -= OnCompleteConversation;
		}

		protected override void OnAction (object sender, InfoEventArgs<BattleInputs> e) {
			base.OnAction(sender, e);
			conversationController.Next();
		}

		private void OnCompleteConversation (object sender, System.EventArgs e) {
			NextState();
		}

		private void NextState () {
			if (IsBattleOver()) {
				owner.ChangeState<EndBattleState>();
			} else {
				owner.ChangeState<SelectUnitState>();
			}
		}

		private IEnumerator NextStateCoroutine () {
			yield return null;
			NextState();
		}

	}

}
