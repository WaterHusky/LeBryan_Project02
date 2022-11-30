using UnityEngine;
using System.Collections.Generic;
using Tactical.Core.Enums;
using Tactical.Core.Extensions;
using Tactical.Actor.Component;
using Tactical.AI.Component;
using Tactical.AI.Model;
using Tactical.Battle.Controller;
using Tactical.Grid.Component;

namespace Tactical.AI.Controller {

	public class ComputerPlayerController : MonoBehaviour {

		private BattleController bc;
		private Unit actor {
			get { return bc.turn.actor; }
		}
		private Alliance alliance {
			get { return actor.GetComponent<Alliance>(); }
		}
		private Unit nearestFoe;

		private void Awake () {
			bc = GetComponent<BattleController>();
		}

		public PlanOfAttack Evaluate () {
			var poa = new PlanOfAttack();
			AttackPattern pattern = actor.GetComponentInChildren<AttackPattern>();
			if (pattern) {
				pattern.Pick(poa);
			} else {
				// TODO: Remove default pattern and throw instead?
				DefaultAttackPattern(poa);
			}

			if (IsPositionIndependent(poa)) {
				PlanPositionIndependent(poa);
			} else if (IsDirectionIndependent(poa)) {
				PlanDirectionIndependent(poa);
			} else {
				PlanDirectionDependent(poa);
			}

			if (poa.ability == null) {
				// TODO: Flee if low HP.
				MoveTowardOpponent(poa);
			}


			return poa;
		}

		/// <summary>
		/// Gets the nearest foe and returns the direction to it.
		/// </summary>
		///
		/// <returns>The direction to the nearest foe.</returns>
		public Directions DetermineEndFacingDirection () {
			var dir = (Directions) Random.Range(0, 4);
			UpdateNearestFoe();

			if (nearestFoe != null) {
				Directions start = actor.dir;
				for (int i = 0; i < 4; ++i) {
					actor.dir = (Directions)i;
					if (nearestFoe.GetFacing(actor) == Facings.Front) {
						dir = actor.dir;
						break;
					}
				}
				actor.dir = start;
			}
			return dir;
		}

		private void PlanDirectionIndependent (PlanOfAttack poa) {
			Tile startTile = actor.tile;
			var map = new Dictionary<Tile, AttackOption>();
			AbilityRange abilityRange = poa.ability.GetComponent<AbilityRange>();
			List<Tile> moveOptions = GetMoveOptions();

			// Loop on the move options (movement range).
			for (int i = 0; i < moveOptions.Count; ++i) {
				Tile moveTile = moveOptions[i];
				actor.Place(moveTile);
				List<Tile> fireOptions = abilityRange.GetTilesInRange(bc.board);

				// Loop on the fire options (ability range).
				for (int j = 0; j < fireOptions.Count; ++j) {
					Tile fireTile = fireOptions[j];
					AttackOption attackOption = null;
					if (map.ContainsKey(fireTile)) {
						attackOption = map[fireTile];
					} else {
						attackOption = new AttackOption();
						attackOption.target = fireTile;
						attackOption.direction = actor.dir;
						map[fireTile] = attackOption;
						RateFireLocation(poa, attackOption);
					}

					attackOption.AddMoveTarget(moveTile);
				}
			}

			// Place the actor back to the initial tile. If we didn't do this, the
			// AI would be out of sync with the visuals.
			actor.Place(startTile);
			var list = new List<AttackOption>(map.Values);
			PickBestOption(poa, list);
		}

		private void PlanDirectionDependent (PlanOfAttack poa) {
			Tile startTile = actor.tile;
			Directions startDirection = actor.dir;
			var list = new List<AttackOption>();
			List<Tile> moveOptions = GetMoveOptions();

			// Loop on the move options (movement range)
			for (int i = 0; i < moveOptions.Count; ++i) {
				Tile moveTile = moveOptions[i];
				actor.Place(moveTile);

				// Loop on the directions.
				for (int j = 0; j < 4; ++j) {
					actor.dir = (Directions)j;
					var actionOption = new AttackOption();
					actionOption.target = moveTile;
					actionOption.direction = actor.dir;
					RateFireLocation(poa, actionOption);
					actionOption.AddMoveTarget(moveTile);
					list.Add(actionOption);
				}
			}

			// Place the actor back to the initial tile. If we didn't do this, the
			// AI would be out of sync with the visuals.
			actor.Place(startTile);
			actor.dir = startDirection;
			PickBestOption(poa, list);
		}

		private void RateFireLocation (PlanOfAttack poa, AttackOption attackOption) {
			AbilityArea area = poa.ability.GetComponent<AbilityArea>();
			List<Tile> tiles = area.GetTilesInArea(bc.board, attackOption.target.pos);
			attackOption.areaTargets = tiles;
			attackOption.isCasterMatch = IsAbilityTargetMatch(poa, actor.tile);

			for (int i = 0; i < tiles.Count; ++i) {
				Tile tile = tiles[i];
				if (actor.tile == tiles[i] || !poa.ability.IsTarget(tile)) {
					continue;
				}

				bool isMatch = IsAbilityTargetMatch(poa, tile);
				attackOption.AddMark(tile, isMatch);
			}
		}

		/// <summary>
		/// Picks the best option from a list of attack options using the score of
		/// each option.
		/// - If a best option is found (score > 0), the plan of attack is updated
		/// with the necessary locations and direction.
		/// - If there is not best option, the plan of attack's ability is set
		/// to null and nothing else is done.
		/// </summary>
		///
		/// <param name="poa">The plan of attack.</param>
		/// <param name="list">The list of options.</param>
		private void PickBestOption (PlanOfAttack poa, List<AttackOption> list) {
			int bestScore = 1;
			var bestOptions = new List<AttackOption>();

			// Loop on all the options.
			for (int i = 0; i < list.Count; ++i) {
				AttackOption option = list[i];
				int score = option.GetScore(actor, poa.ability);

				// If the option's score is better than the current best, replace the
				// best and keep only it in the best options list.
				if (score > bestScore) {
					bestScore = score;
					bestOptions.Clear();
					bestOptions.Add(option);
				}
				// If the option's score is the same as the current best, simply add
				// it to the best options list.
				else if (score == bestScore) {
					bestOptions.Add(option);
				}
			}

			// If there is not best options, clear the plan of attack's ability and
			// stop here.
			if (bestOptions.Count == 0) {
				poa.ability = null;
				return;
			}

			var finalOptions = new List<AttackOption>();
			bestScore = 0;

			// Loop on the best options.
			// TODO: Use the same method to get the best options and the final options (refactor).
			for (int i = 0; i < bestOptions.Count; ++i) {
				AttackOption option = bestOptions[i];
				int score = option.bestAngleBasedScore;
				if (score > bestScore) {
					bestScore = score;
					finalOptions.Clear();
					finalOptions.Add(option);
				} else if (score == bestScore) {
					finalOptions.Add(option);
				}
			}

			// Pick a random option and use it to populate the plan of attack's fields.
			AttackOption finalOption = finalOptions[Random.Range(0, finalOptions.Count)];
			poa.fireLocation = finalOption.target.pos;
			poa.attackDirection = finalOption.direction;
			poa.moveLocation = finalOption.bestMoveTile.pos;
		}

		/// <summary>
		/// Updates the nearestFoe property to be the first foe that is not dead.
		///
		/// @todo Replace this with something that takes into account the
		/// HP / debuffs / armor of the targets to find the highest priority target.
		/// </summary>
		private void UpdateNearestFoe () {
			nearestFoe = null;
			bc.board.Search(actor.tile, delegate(Tile tile, Tile otherTile) {

				// If the other tile is a foe and alive, set it as the nearest for and return true.
				if (nearestFoe == null && otherTile.content != null) {
					Alliance otherAlliance = otherTile.content.GetComponentInChildren<Alliance>();
					if (otherAlliance != null && alliance.IsMatch(otherAlliance, Targets.Foe)) {
						Unit unit = otherAlliance.GetComponent<Unit>();
						Stats stats = unit.GetComponent<Stats>();
						if (stats[StatTypes.HP] > 0) {
							nearestFoe = unit;
							return true;
						}
					}
				}

				return nearestFoe == null;
			});
		}

		/// <summary>
		/// Sets the plan of attack's move location to the closest tile possible to
		/// the nearest foe.
		///
		/// @todo What if not moving is the better move?
		/// </summary>
		///
		/// <param name="poa">The plan of attack.</param>
		private void MoveTowardOpponent (PlanOfAttack poa) {
			List<Tile> moveOptions = GetMoveOptions();
			UpdateNearestFoe();

			if (nearestFoe != null) {
				Tile toCheck = nearestFoe.tile;
				while (toCheck != null) {
					if (moveOptions.Contains(toCheck)) {
						poa.moveLocation = toCheck.pos;
						return;
					}
					toCheck = toCheck.prev;
				}
			}

			poa.moveLocation = actor.tile.pos;
		}

		/// <summary>
		/// Determines if the plan of attack's ability is position independent.
		/// </summary>
		///
		/// <param name="poa">The plan of attack.</param>
		///
		/// <returns>True if the ability is position independent, False otherwise.</returns>
		private bool IsPositionIndependent (PlanOfAttack poa) {
			var range = poa.ability.GetComponent<AbilityRange>();
			return !range.positionOriented;
		}

		/// <summary>
		/// Determines if the plan of attack's ability is direction independent.
		/// </summary>
		///
		/// <param name="poa">The plan of attack.</param>
		///
		/// <returns>True if the ability is direction independent, False otherwise.</returns>
		private bool IsDirectionIndependent (PlanOfAttack poa) {
			var range = poa.ability.GetComponent<AbilityRange>();
			return !range.directionOriented;
		}

		/// <summary>
		/// Sets the plan of attack's move location and fire location to
		/// a random position (within the possible move options).
		/// </summary>
		///
		/// <param name="poa">The plan of attack.</param>
		private void PlanPositionIndependent (PlanOfAttack poa) {
			List<Tile> moveOptions = GetMoveOptions();
			Tile tile = moveOptions[Random.Range(0, moveOptions.Count)];
			poa.moveLocation = poa.fireLocation = tile.pos;
		}

		/// <summary>
		/// Gets the move options for the actor.
		///
		/// @todo What if the the actor has no movement component?
		/// </summary>
		///
		/// <returns>The move options (list of tiles).</returns>
		private List<Tile> GetMoveOptions () {
			return actor.GetComponent<Movement>().GetTilesInRange(bc.board);
		}

		/// <summary>
		/// Determines if the ability target's is a match (should use ability).
		/// </summary>
		///
		/// <param name="poa">The plan of attack.</param>
		/// <param name="tile">The tile to check.</param>
		///
		/// <returns>True if the ability target's is a match , False otherwise.</returns>
		private bool IsAbilityTargetMatch (PlanOfAttack poa, Tile tile) {
			bool isMatch = false;

			// If the target is a Tile, it's automatically a match.
			if (poa.target == Targets.Tile) {
				isMatch = true;
			}
			// If the target is a Unit (Ally, Foe, Self), use the unit's alliance
			// to determine if it's a match.
			else if (poa.target != Targets.None) {
				Alliance other = tile.content.GetComponentInChildren<Alliance>();
				if (other != null && alliance.IsMatch(other, poa.target)) {
					isMatch = true;
				}
			}

			return isMatch;
		}

		/// <summary>
		/// Sets the default attack pattern on the plan of attack.
		///
		/// @todo What if the the actor has no ability?
		/// </summary>
		///
		/// <param name="poa">The plan of attack.</param>
		private void DefaultAttackPattern (PlanOfAttack poa) {
			poa.ability = actor.GetComponentInChildren<Ability>();
			poa.target = Targets.Foe;
		}

	}

}
