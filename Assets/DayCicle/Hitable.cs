using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public enum HitableType { Wood, Rock, Animal, Viking }

public class Hitable : MonoBehaviour
{
    public HitableType Type;
    public int Health;
    public UnityEvent OnHit;
    public UnityEvent OnDeath;

    public GameObject DropItemPref;

    public bool SpawnDmgPref;
    public GameObject DamagePref;

    public void Hit(int dmg)
    {
        Health -= dmg;

        if (SpawnDmgPref)
        {
            SpawnDamagePref(dmg);
        }

        if (Health <= 0)
        {
            SpawnItem();

            OnDeath.Invoke();
        }
    }

    private void SpawnItem()
    {
        if (!DropItemPref)
        {
            return;
        }

        Instantiate(DropItemPref, transform.position, Quaternion.identity);
    }

    private void SpawnDamagePref(int dmg)
    {
        if (!SpawnDmgPref)
        {
            return;
        }

        GameObject go = Instantiate(DamagePref, transform.position, Quaternion.identity);
        go.GetComponentInChildren<TextMesh>().text = dmg.ToString();

        Destroy(go, 0.2f);
    }
}
