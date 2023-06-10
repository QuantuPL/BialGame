using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;

public class MainMenu : MonoBehaviour
{
    private static int currentLevel = 0;

    public Button ContinueBut;
    public Image FadeScreen;
    public TextMeshProUGUI FirstLevel;


    private bool lockInput = false;

    void Start()
    {
        EventSystem.current.firstSelectedGameObject = ContinueBut.gameObject;
        Time.timeScale = 1;
        FadeScreen.color = Color.black;
        FadeScreen.DOFade(0, 2);

        currentLevel = 1;
        if (LoadScore() == 0)
        {
            FirstLevel.text = "";
        } else if (LoadScore() == 1)
        {
            FirstLevel.text = $"Best run: 1 day";
        } else
        {
            FirstLevel.text = $"Best run: {LoadScore()} days";
        }
    }

    public void StartGame()
    {
        if (lockInput)
        {
            return;
        }

        lockInput = true;
        FadeScreen.DOFade(1, 2).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(1);
        });
    }

    public void Exit()
    {
        if (lockInput)
        {
            return;
        }

        lockInput = true;
        FadeScreen.DOFade(1, 2).OnComplete(() =>
        {
            Application.Quit();
        });
    }

    public void LoadLevel(int lvl)
    {

        if (lockInput)
        {
            return;
        }

        lockInput = true;

        currentLevel = lvl;
        FadeScreen.DOFade(1, 2).OnComplete(() =>
        {
            SceneManager.LoadSceneAsync(lvl);
        });
    }

    public static int LoadScore()
    {
        return PlayerPrefs.GetInt($"bestScore{currentLevel}", 0);
    }

    public static void SaveScore(int days)
    {
        int score = LoadScore();

        if (score >= days)
        {
            return;
        }

        PlayerPrefs.SetInt($"bestSocre{currentLevel}", days);
        PlayerPrefs.Save();
    }
}
