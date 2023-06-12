using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InfoViewer : MonoBehaviour
{
    [SerializeField] private CanvasGroup Display;
    [SerializeField] private Image Glow;
    [SerializeField] private Image Thumbnail;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI Components;

    [SerializeField] private CanvasGroup RuneDisplay;
    [SerializeField] private Image RuneGlow;
    [SerializeField] private TextMeshProUGUI RuneName;
    [SerializeField] private TextMeshProUGUI RuneDescription;


    void Start()
    {
        Display.alpha = 0;
        Glow.color = new Color(1, 1, 1, 0);
        Glow.rectTransform.localScale = Vector3.one;
    }

    [ContextMenu("Test")]
    void Test()
    {
        ShowNewRecipe(null);
    }

    public void ShowNewRecipe(CraftingRecipe recipe)
    {
        Glow.DOFade(1, 1.2f);
        Glow.DOFade(0, 0.7f).SetDelay(1.2f);
        Glow.rectTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 1.5f, 0, 0);
        Display.DOFade(1, 0.5f).SetDelay(1.1f);
        Display.DOFade(0, 0.5f).SetDelay(6);

        ItemName.text = recipe.title;
        Thumbnail.sprite = recipe.thumbnail;
        if (recipe.item1 == recipe.item2)
        {
            Components.text = $"2 x {recipe.item1}";
        }
        else if (!string.IsNullOrWhiteSpace(recipe.item2))
        {
            Components.text = $"1 x {recipe.item1}  1 x {recipe.item2}";

        }
        else
        {
            Components.text = $"1 x {recipe.item1}";
        }

    }

    public void ShowRune(string title, string description)
    {
        Glow.DOFade(1, 1.2f);
        Glow.DOFade(0, 0.7f).SetDelay(1.2f);
        Glow.rectTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 1.5f, 0, 0);
        RuneDisplay.DOFade(1, 0.5f).SetDelay(1.1f);
        Display.DOFade(0, 0.5f).SetDelay(6);
    }
}
