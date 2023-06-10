using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    public PlaceSpot placeSpot;
    public CraftingMethod method;
    private Pickable lastItem;
    private float timeRemaining;

    public CraftingRecipe currentRecipe;
    void Update()
    {
        if (lastItem != placeSpot.Item)
        {
            if (placeSpot.Item != null)
            {
                var itemName = placeSpot.Item.name.Replace("(Clone)", "");
                var temp = RecipeeBook.AllRecipees();
                foreach (var craftingRecipe in temp)
                {
                    if (craftingRecipe.craftingMethod == method)
                    {
                        if (craftingRecipe.item1.Contains(itemName))
                        {
                            currentRecipe = craftingRecipe;
                            timeRemaining = currentRecipe.time;
                            break;
                        }
                    }
                }
            }   
        }

        if (currentRecipe != null)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                Craft();
            }
        }

        lastItem = placeSpot.Item;
    }
    
    public void Craft()
    {
        var item1 = placeSpot.Item;
        
        placeSpot.Drop(Vector3.zero);
        
        Destroy(item1.gameObject);

        var crafted = Instantiate(currentRecipe.creates).GetComponent<Pickable>();
        
        placeSpot.Pick(crafted);

        currentRecipe = null;
    }
}
