using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public HitableType type;
    public int life;
    public int armor;
    public object lastInflictedBy;

    public UnityEvent<Health> OnDamaged;
    public UnityEvent<Health> OnDeath;

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