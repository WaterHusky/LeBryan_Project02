using UnityEditor;
using Tactical.Actor.Factory;

namespace Tactical.Core.Editor {

	public static class RecipeCreator {

		[MenuItem("Assets/Create/Unit Recipe")]
		public static void CreateUnitRecipe () {
			ScriptableObjectUtility.CreateAsset<UnitRecipe>();
		}

		[MenuItem("Assets/Create/Ability Catalog Recipe")]
		public static void CreateAbilityCatalogRecipe () {
			ScriptableObjectUtility.CreateAsset<AbilityCatalogRecipe>();
		}

	}

}
