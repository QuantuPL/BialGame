using UnityEngine;

[CreateAssetMenu(menuName = "Crafting Recipe", fileName = "New Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public CraftingMethod craftingMethod;
    
    public string item1;
    public string item2;
    public float time;

    public GameObject creates;
}

public enum CraftingMethod
{
    Furnace, CraftingStation, Campfire
}
