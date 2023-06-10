using System.Collections.Generic;
using UnityEngine;

public class RecipeeBook : MonoBehaviour
{
    public List<CraftingRecipe> recipees;

    public static RecipeeBook instance;

    private void Start()
    {
        instance = this;
    }

    public static List<CraftingRecipe> AllRecipees()
    {
        return instance.recipees;
    }
}
