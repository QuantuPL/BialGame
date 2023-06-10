using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public enum HitableType { Wood, Rock, Animal, Viking, Player }

public class Hitable : MonoBehaviour
{
    public HitableType Type;
    public int Health;
    public UnityEvent OnHit;
    public UnityEvent OnDeath;

    public GameObject DropItemPref;

    public bool SpawnDmgPref;
    public GameObject DamagePref;

    public void Hit(int dmg, Player player = null)
    {
        print("hit");
        Health -= dmg;

        if (SpawnDmgPref)
        {
            SpawnDamagePref(dmg);
        }

        switch (Type)
        {
            case HitableType.Player:
                Health += Mathf.Max(0, dmg - 1);
                break;
            case HitableType.Viking:
                GetComponent<Viking>().AgroPlayer = player;
                break;
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
