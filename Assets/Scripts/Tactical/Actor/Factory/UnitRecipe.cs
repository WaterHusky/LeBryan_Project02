using UnityEngine;
using Tactical.Core.Enums;

namespace Tactical.Actor.Factory {

	public class UnitRecipe : ScriptableObject {

		public string model;
		public string job;
		public string attack;
		public string abilityCatalog;
		public MovementTypes movementType;
		public Alliances alliance;
		public string strategy;

	}

}
