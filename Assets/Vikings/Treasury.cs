using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Treasury : MonoBehaviour
{
    public static Vector3 pos => Instance.transform.position;
    public static Treasury Instance;

    public int GoldCount = 10;

    public GameObject GoldPref;

    void Awake()
    {
        Instance = this;
    }

    public void TakeGold ()
    {
        GoldCount--;

        if (GoldCount <= 0)
        {
            print("Game over");
            //TODO end screen
        }
    }

    public void LostGold ()
    {
        //TODO sound when viking get gold to boat
    }
}
