using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public Transform replenished, depleted;

    public bool isDepleted;

    public GameObject drop;

    public Health health;

    public int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        replenished.gameObject.SetActive(true);
        depleted.gameObject.SetActive(false);
    }

    public void Hit(Health health)
    {
        if (!isDepleted)
        {
            replenished.DOShakePosition(0.3f, Vector3.left * 0.2f);
        }
    }

    public void Death(Health health)
    {
        if (!isDepleted)
        {
            isDepleted = true;
            replenished.gameObject.SetActive(false);
            depleted.gameObject.SetActive(true);
            var item = Instantiate(drop);
            item.transform.position = transform.position;
            item.transform.DoAnimateItem();
        }
    }

    public void Replenish()
    {
        health.life = maxHealth;
        isDepleted = false;
        replenished.gameObject.SetActive(true);
        depleted.gameObject.SetActive(false);
    }
}
