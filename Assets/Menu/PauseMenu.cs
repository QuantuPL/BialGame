using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Image FadeScreen;
    public Button ContinueButton;
    public RectTransform NormalUI;
    public RectTransform PauseUI;

    private bool isOpen = false;
    private bool lockInput = false;
    void Start()
    {
        FadeScreen.color = Color.black;
        FadeScreen.DOFade(0, 2);

        NormalUI.gameObject.SetActive(true);
        PauseUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isOpen = !isOpen;
            Time.timeScale = isOpen ? 0 : 1;
            NormalUI.gameObject.SetActive(!isOpen);
            PauseUI.gameObject.SetActive(isOpen);

            if (isOpen)
            {
                ContinueButton.Select();
            }
        }
    }

    public void Continue()
    {
        Time.timeScale = 1;
        isOpen = false;
        NormalUI.gameObject.SetActive(!isOpen);
        PauseUI.gameObject.SetActive(isOpen);
    }

    public void ResetScene()
    {
        if (lockInput)
        {
            return;
        }

        lockInput = true;
        FadeScreen.DOFade(1, 2).OnComplete(() => {
            Time.timeScale = 1;
            var scene = SceneManager.GetActiveScene();

            SceneManager.LoadSceneAsync(scene.buildIndex);
        });
    }

    public void Exit()
    {
        if (lockInput)
        {
            return;
        }

        lockInput = true;
        FadeScreen.DOFade(1, 2).OnComplete(() => {
            Time.timeScale = 1;
            
            SceneManager.LoadSceneAsync(0);
        });
    }
}
