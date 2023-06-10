using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class Treasury : MonoBehaviour
{
    public static Vector3 pos => Instance.transform.position;
    public static Treasury Instance;

    public int GoldCount = 10;

    public GameObject GoldPref;

    public TextMeshProUGUI GoldCounter;

    void Awake()
    {
        Instance = this;

        GoldCounter.text = GoldCount.ToString();
    }

    public void TakeGold ()
    {
        if (GoldCount <=0)
        {
            return;
        }

        GoldCount--;
        GoldCounter.text = GoldCount.ToString();

        if (GoldCount <= 0)
        {
            print("Game over");

            FindObjectOfType<EndScreen>().ShowEnd();
        }
    }

    public void LostGold ()
    {
        //TODO sound when viking get gold to boat
    }
}
