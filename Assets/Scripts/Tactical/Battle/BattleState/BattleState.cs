using UnityEngine;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.EventArgs;
using Tactical.Core.StateMachine;
using Tactical.Core.Controller;
using Tactical.Grid.Model;
using Tactical.Grid.Component;
using Tactical.Actor.Component;
using Tactical.Battle.Model;
using Tactical.Battle.Controller;
using Tactical.Battle.Component;
using Tactical.UI.Controller;

namespace Tactical.Battle.BattleState {

	public abstract class BattleState : State {

		public CameraRig cameraRig {
			get { return owner.cameraRig; }
		}
		public Board board {
			get { return owner.board; }
		}
		public LevelData levelData {
			get { return owner.levelData; }
		}
		public Transform tileSelectionIndicator {
			get { return owner.tileSelectionIndicator; }
		}
		public Point pos {
			get { return owner.pos; }
			set { owner.pos = value; }
		}
		public ActionMenuPanelController actionMenuPanelController {
			get { return owner.actionMenuPanelController; }
		}
		public TurnOrderPanelController turnOrderPanelController {
			get { return owner.turnOrderPanelController; }
		}
		public HitIndicatorPanelController hitIndicatorPanelController {
			get { return owner.hitIndicatorPanelController; }
		}
		public StatPanelController statPanelController {
			get { return owner.statPanelController; }
		}
		public UnitDirectionController unitDirectionController {
			get { return owner.unitDirectionController; }
		}
		public Turn turn {
			get { return owner.turn; }
		}
		public List<Unit> units {
			get { return owner.units; }
		}

		protected BattleController owner;
		protected Driver driver;

		protected virtual void Awake () {
			owner = GetComponent<BattleController>();
		}

		public override void Enter () {
			driver = (turn.actor != null) ? turn.actor.GetComponent<Driver>() : null;
			base.Enter ();
		}

		protected override void AddListeners () {
			if (driver == null || driver.Current == Drivers.Human || IsBattleOver())
			{
				BattleInputController.MoveEvent += OnMove;
				BattleInputController.ActionEvent += OnAction;
			}
		}

		protected override void RemoveListeners () {
			BattleInputController.MoveEvent -= OnMove;
			BattleInputController.ActionEvent -= OnAction;
		}

		protected virtual void OnMove (object sender, InfoEventArgs<Point> e) {}

		protected virtual void OnAction (object sender, InfoEventArgs<BattleInputs> e) {
			switch (e.info) {
			case BattleInputs.RotateCameraLeft:
				owner.cameraRig.rotateTowards += Vector3.up * 90f;
				break;
			case BattleInputs.RotateCameraRight:
				owner.cameraRig.rotateTowards += Vector3.up * -90f;
				break;
			}
		}

		protected virtual void SelectTile (Point p) {
			if (pos == p || !board.tiles.ContainsKey(p)) {
				return;
			}

			// Play the selection clip.
			if (owner.audioSource && owner.tileSelectionClip) {
				if (driver && driver.Current == Drivers.Human) {
					owner.audioSource.clip = owner.tileSelectionClip;
					owner.audioSource.Play();
				}
			}

			pos = p;
			tileSelectionIndicator.localPosition = board.tiles[p].center;
		}

		protected virtual Unit GetUnit (Point p) {
			Tile t = board.GetTile(p);
			GameObject content = t != null ? t.content : null;
			return content != null ? content.GetComponent<Unit>() : null;
		}

		protected virtual void RefreshPrimaryStatPanel (Point p) {
			var target = GetUnit(p);
			if (target != null) {
				statPanelController.ShowPrimary(target.gameObject);
			} else {
				statPanelController.HidePrimary();
			}
		}

		protected virtual void RefreshSecondaryStatPanel (Point p) {
			var target = GetUnit(p);
			if (target != null) {
				statPanelController.ShowSecondary(target.gameObject);
			} else {
				statPanelController.HideSecondary();
			}
		}

		protected virtual bool DidPlayerWin () {
			return owner.GetComponent<BaseVictoryCondition>().Victor == Alliances.Hero;
		}

		protected virtual bool IsBattleOver () {
			return owner.GetComponent<BaseVictoryCondition>().Victor != Alliances.None;
		}

	}

}
