using UnityEngine;

namespace Tactical.Actor.Factory {

	public class AbilityCatalogRecipe : ScriptableObject {

		[System.Serializable]
		public class Category {
			public string name;
			public string[] entries;
		}
		[NonReorderable]
		public Category[] categories;

	}

}
