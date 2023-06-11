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

    public TextMeshProUGUI GoldCounter;

    private Picking picking;

    void Awake()
    {
        Instance = this;

        picking = GetComponent<Picking>();

        GoldCounter.text = GoldCount.ToString();
    }

    void Update()
    {
        AddGold();
    }

    private void AddGold()
    {
        Pickable pickable = picking.Item;

        if (!pickable)
        {
            return;
        }

        if (!pickable.name.Contains ("Gold Nugget"))
        {
            picking.Drop(transform.position);

            pickable.transform.DoAnimateItem();

            return;
        }

        Destroy(pickable.gameObject);

        GoldCount++;
        GoldCounter.text = GoldCount.ToString();
    }

    public void TakeGold()
    {
        if (GoldCount <= 0)
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

    public void LostGold()
    {
        //TODO sound when viking get gold to boat
    }
}
