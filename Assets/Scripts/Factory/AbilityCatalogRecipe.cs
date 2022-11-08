using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCatalogRecipe : ScriptableObject
{
    [System.Serializable] 
    public class Category
    {
        public string name;
        public string[] entries;
    }
    [NonReorderable]
    public Category[] categories;
}
