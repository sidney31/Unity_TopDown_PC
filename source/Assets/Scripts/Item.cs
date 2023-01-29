using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{
    public ItemType type;
    public bool stackable = true;
    public Sprite image;
    public Recipe[] recipe;


    public bool CheckCraftPossibility()
    {
        bool result = false;

        return result;
    }

    [System.Serializable]
    public struct Recipe
    {
        public Item _itemName;
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