using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public CanvasGroup bg;
    public CanvasGroup Body;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI NewRecord;

    public Button RetryBut;

    void Start()
    {
        bg.transform.parent.gameObject.SetActive(false);
        bg.alpha = 0;
        bg.interactable = false;
        bg.blocksRaycasts = false;

        NewRecord.transform.localScale = Vector3.zero;
        Title.alpha = 0;
        Title.rectTransform.DOLocalMoveY(600, 0);
        Body.transform.localScale = Vector3.one * 0.8f;
        Body.alpha = 0;
        DayText.alpha = 0;
        DayText.transform.localScale = Vector3.zero;
    }

    [ContextMenu("Show")]
    public void ShowEnd()
    {
        FindObjectOfType<PauseMenu>().enabled = false;
        bg.transform.parent.gameObject.SetActive(true);
        bg.interactable = true;
        bg.blocksRaycasts = true;
        
        RetryBut.Select();

        bg.DOFade(1, 1.5f);

        Title.DOFade(1, 1).SetDelay(0.5f);
        Title.rectTransform.DOLocalMoveY(490, 2f);

        Body.transform.DOScale(Vector3.one, 1).SetDelay(1.5f);
        Body.DOFade(1, 1).SetDelay(1.5f);

        DayText.DOFade(1, 1).SetDelay(3);
        DayText.transform.DOScale(Vector3.one, 1).SetDelay(3);
        DayText.transform.DOLocalMoveY(160, 1).SetDelay(3);

        int score = MainMenu.LoadScore();
        int s = FindObjectOfType<TheGame>().currentDay;
        if (s > score)
        {
            NewRecord.transform.DOScale(Vector3.one, 0.5f).SetDelay(4);
            MainMenu.SaveScore(s);
        }
    }
}
