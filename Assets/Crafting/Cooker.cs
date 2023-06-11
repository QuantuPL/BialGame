using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    public CraftingMethod method;
    private Pickable lastItem;
    private float timeRemaining;
    private Picking picking;
    public CraftingRecipe currentRecipe;

    void Start()
    {
        picking = GetComponent<Picking>();
    }

    void Update()
    {
        if (picking.Item && picking.Item != lastItem)
        {
            var itemName = picking.Item.name.Replace("(Clone)", "");
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

        if (currentRecipe != null)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                Craft();
            }
        }

        lastItem = picking.Item;
    }
    
    public void Craft()
    {
        var item1 = picking.Item;
        
        picking.Drop(Vector3.zero);
        
        Destroy(item1.gameObject);

        var crafted = Instantiate(currentRecipe.creates).GetComponent<Pickable>();
        
        picking.Take(crafted);

        currentRecipe = null;
    }
}
