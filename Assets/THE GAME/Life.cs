using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public HitableType type;
    public int maxLife = 3;
    public int life;
    public int armor;
    public object lastInflictedBy;

    public UnityEvent<Health> OnDamaged;
    public UnityEvent<Health> OnDeath;

    public bool ShowHealth = false;
    public Transform Holder;
    public GameObject HeartPref;

    private void Start()
    {
        Holder.gameObject.SetActive(ShowHealth);
        if (ShowHealth)
        {
            SetHealth(life);
        }
        Holder.gameObject.SetActive(!ShowHealth);
        Holder.gameObject.SetActive(ShowHealth);
    }

    public void SetHealth (int health)
    {
        for (int i = 0; i < health; i++)
        {
            GameObject go = Instantiate(HeartPref, Holder);
            go.GetComponent<Image>().color = i < maxLife ? Color.red : Color.yellow;
        }
    }

    public void Damage(int dmg, bool destroysArmor, object infilctedBy)
    {
        if (destroysArmor)
        {
            armor = 0;
        }
        life -= (dmg - armor);
        lastInflictedBy = infilctedBy;


        if (life <= 0)
        {
            if (ShowHealth)
            {
                for (int i = Holder.childCount - 1; i >= 0; i--)
                {
                    Destroy(Holder.GetChild(i).gameObject);
                }
            }
            OnDeath.Invoke(this);
        }
        else
        {
            if (ShowHealth)
            {
                for (int i = life + dmg - armor; i >= life; i--)
                {
                    Destroy(Holder.GetChild(i).gameObject);
                }
            }
            OnDamaged.Invoke(this); 
        }
    }
}

public enum HitableType
{
    Flesh, Stone, Wood,
}