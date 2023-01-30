using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    public ItemType type;
    public bool stackable = true;
    public Sprite image;
    public Recipe[] recipe;


    public bool CheckCraftPossibility(Dictionary<Item, int> ItemCraftRecipe)
    {
        bool result = false;
        Debug.Log(recipe.Length);
        foreach(Recipe resource in recipe)
        {
            ItemCraftRecipe.TryGetValue(resource.itemName, out int value);

            if (value >= resource.count)
            {
                result = true;
            }
            else
            {
                result = false;
                break;
            }
        }
        return result;
    }

    [System.Serializable]
    public struct Recipe
    {
        public Item itemName;
        public int count;
    }
}

public enum ItemType
{
    sword,
    axe,
    pickaxe,
    shovel,
    block,
    eat,
    seed,
}