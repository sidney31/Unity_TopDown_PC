using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class Item : ScriptableObject
{

    public ItemType type;
    public bool stackable = true;
    public Sprite image;
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