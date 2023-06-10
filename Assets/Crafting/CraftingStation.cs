using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingStation : MonoBehaviour
{
    public PlaceSpot placeSpot1, placeSpot2;
    public List<CraftingRecipe> availableRecipees;
    public int index;
    public CraftingUI cui;
    void Update()
    {            
        availableRecipees.Clear();

        if (placeSpot1.Item == null || placeSpot2.Item == null)
            return;
        var itemName1 = placeSpot1.Item.name.Replace("(Clone)", "");
        var itemName2 = placeSpot2.Item.name.Replace("(Clone)", "");

        var temp = RecipeeBook.AllRecipees();
        foreach (var craftingRecipe in temp)
        {
            if (craftingRecipe.craftingMethod != CraftingMethod.CraftingStation)
                continue;
            
            if (craftingRecipe.item1.Contains(itemName1) && craftingRecipe.item2.Contains(itemName2))
                availableRecipees.Add(craftingRecipe);
            else if (craftingRecipe.item2.Contains(itemName1) && craftingRecipe.item1.Contains(itemName2))
                availableRecipees.Add(craftingRecipe);

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Craft();
        }
    }

    public CraftingUI StartUsing()
    {
        if (availableRecipees.Count == 0)
        {
            return null;
        }
        cui.Use();
        return cui;
    }

    public void StopUsing()
    {
        cui.StopUse();
    }

    public void Craft()
    {
        var item1 = placeSpot1.Item;
        var item2 = placeSpot2.Item;
        
        placeSpot1.Drop(Vector3.zero);
        placeSpot2.Drop(Vector3.zero);
        
        Destroy(item1.gameObject);
        Destroy(item2.gameObject);

        var recipe = availableRecipees[index];

        var crafted = Instantiate(recipe.creates).GetComponent<Pickable>();
        
        placeSpot1.Take(crafted);
        StopUsing();
    }
}
