using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public List<RectTransform> panels;
    public GameObject ui;
    public int index = 0;
    public CraftingStation cs;

    private int availableRecipees;

    public void UpdateOnIndexes()
    {
        for (int i = 0; i < 5; i++)
        {
            CraftingRecipe cr = cs.availableRecipees[(index + i) % availableRecipees];

            panels[i].GetComponent<Image>().sprite = cr.creates.GetComponent<SpriteRenderer>().sprite;
        }

        return;
        var amountOfRecipes = cs.availableRecipees.Count;
        var wantedSprite = cs.availableRecipees[((index+3)%amountOfRecipes + amountOfRecipes) % amountOfRecipes].creates.GetComponent<SpriteRenderer>().sprite;
        panels[((-index+3)%5 + 5) % 5].GetComponent<Image>().sprite = wantedSprite;
    }
    
    public void Use()
    {
        /*
        index = 0;

        for (int i = 0; i < 5; i++)
        {
            index++;
            UpdateOnIndexes();
        }
        */

        ui.SetActive(true);

        availableRecipees = cs.availableRecipees.Count;

        index = availableRecipees * 5;

        for (int i = 0; i < 5; i++)
        {
            CraftingRecipe cr = cs.availableRecipees[(index + i) % availableRecipees];

            panels[i].GetComponent<Image>().sprite = cr.creates.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public void StopUse()
    {
        ui.SetActive(false);
    }

    public void MoveNext()
    {
        index++;

        if (index%availableRecipees == 0)
        {
            index = availableRecipees * 5;
        }

        for (int i = 0; i < 5; i++)
        {
            var curr = panels[i];
            var next = panels[(i + 1) % 5];
            
            TransformPanel(curr, next);
        }

        RectTransform buf = panels[4];
        for (int i = 4; i >0; i--)
        {
            panels[i] = panels[i - 1];
        }
        panels[0] = buf;

        var amountOfRecipes = cs.availableRecipees.Count;
        cs.index = (index + 2)% availableRecipees; //((index + 3) % amountOfRecipes + amountOfRecipes) % amountOfRecipes;


        UpdateOnIndexes();
    }

    public void MoveLast()
    {
        index--;

        if (index % availableRecipees == 0)
        {
            index = availableRecipees * 5;
        }

        for (int i = 0; i < 5; i++)
        {
            var curr = panels[i];
            var next = panels[(i + 4) % 5];
            
            TransformPanel(curr, next);
        }

        RectTransform buf = panels[0];
        for (int i = 0; i < 4; i++)
        {
            panels[i] = panels[i + 1];
        }
        panels[4] = buf;

        var amountOfRecipes = cs.availableRecipees.Count;
        cs.index = (index + 2) % availableRecipees; //((index + 3) % amountOfRecipes + amountOfRecipes) % amountOfRecipes;
        
        UpdateOnIndexes();
    }

    void TransformPanel(RectTransform from, RectTransform to)
    {
        var position = to.localPosition;
        var rect = to.rect.size;

        from.DOLocalMove(position, 0.1f);
        from.DOSizeDelta(rect, 0.1f);
    }
}
