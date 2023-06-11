using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Revive : MonoBehaviour
{
    public float ReviveTime = 5;
    public float GracePeriod = 2;

    public Transform PlayerVisual;
    public Transform ReviveVisual;

    public Image ReviveImage;
    public TextMeshProUGUI ReviveCounter;

    public Transform RevivePoint;

    private float counter = 0;

    void OnEnable()
    {
        GetComponent<PlayerController>().IsDead = true;

        PlayerVisual.gameObject.SetActive(false);
        ReviveVisual.gameObject.SetActive(true);

        transform.DOMove(RevivePoint.position, ReviveTime).SetEase(Ease.InOutSine);
        counter = ReviveTime;
        ReviveImage.fillAmount = 1;
        ReviveImage.DOFillAmount(0, ReviveTime);
    }

    void OnDisable()
    {
        PlayerVisual.gameObject.SetActive(true);
        ReviveVisual.gameObject.SetActive(false);
    }

    void Update()
    {
        counter -= Time.deltaTime;

        int val = Mathf.CeilToInt(counter);

        ReviveCounter.text = val.ToString();

        if (counter < 0)
        {
            enabled = false;

            GetComponent<Health>().SetHealth(1);
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(GracePeriod);
        GetComponent<PlayerController>().IsDead = false;
    }
}
