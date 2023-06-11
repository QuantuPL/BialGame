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

    public AudioClip LostGoldClip;
    public AudioClip GoldTaken;
    public AudioClip GoldDepo;
    private AudioSource source;

    void Awake()
    {
        Instance = this;

        source = GetComponent<AudioSource>();
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

        source.PlayOneShot(GoldDepo, 1);
    }

    public void TakeGold()
    {
        if (GoldCount <= 0)
        {
            return;
        }

        GoldCount--;
        GoldCounter.text = GoldCount.ToString();

        source.PlayOneShot(GoldTaken, 1);

        if (GoldCount <= 0)
        {
            print("Game over");

            FindObjectOfType<EndScreen>().ShowEnd();
        }
    }

    public void LostGold()
    {
        //TODO sound when viking get gold to boat

        source.PlayOneShot(LostGoldClip, 1);
    }
}
