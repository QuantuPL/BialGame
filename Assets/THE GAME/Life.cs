using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public HitableType type;
    public int maxLife;
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
        maxLife = life;
        if (Holder)
        {
            Holder.gameObject.SetActive(ShowHealth);
        }
        if (ShowHealth)
        {
            SetHealth(life);
            Canvas.ForceUpdateCanvases();
        }
    }

    public void SetHealth (int health)
    {
        life = health;
        if (!ShowHealth)
        {
            return;
        }
        for (int i = Holder.childCount-1; i >= 0; i--)
        {
            Destroy (Holder.GetChild(i).gameObject);
        }

        for (int i = 0; i < health; i++)
        {
            GameObject go = Instantiate(HeartPref, Holder);
            go.GetComponent<Image>().color = i < maxLife ? Color.red : Color.yellow;
        }
    }

    public void Heal ()
    {
        SetHealth (Mathf.Max (maxLife, life));
    }

    public void Damage(int dmg, bool destroysArmor, object infilctedBy)
    {
        if (destroysArmor)
        {
            armor = 0;
        }
        life -= (dmg - armor);
        lastInflictedBy = infilctedBy;

        SetHealth (life);

        if (life <= 0)
        {
            
            OnDeath.Invoke(this);
        }
        else
        {
            
            OnDamaged.Invoke(this); 
        }
    }
}

public enum HitableType
{
    Flesh, Stone, Wood,
}