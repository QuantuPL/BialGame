using DG.Tweening;
using UnityEngine;

public class Berserk : Viking
{
    public float HitDistance = 0.5f;
    public float HitCooldown = 0.3f;

    public int Damage = 1;
    public float Speed = 4;

    private float counter = 0;

    public override void Start()
    {
        base.Start();
        state = State.ToPlayer;
        counter = 0;
    }

    protected override void Update()
    {
        base.Update();
        
        counter += Time.deltaTime;

        Movement();

        Attack();
    }

    private bool HasTarget()
    {
        return AgroPlayer != null;
    }

    private void FindTarget()
    {
        var list = FindObjectsOfType<PlayerController>();
        int index = 0;
        float dist = float.MaxValue;

        for (int i = 0; i < list.Length; i++)
        {
            float d = (list[i].transform.position - transform.position).magnitude;

            if (d < dist)
            {
                dist = d;
                index = i;
            }
        }

        AgroPlayer = list[index];
    }

    private void Movement()
    {
        if (!HasTarget())
        {
            FindTarget();
        }

        var destination = IsFleeing ? Boat.transform.position : AgroPlayer.transform.position;

        var dir = destination - transform.position;
        var distance = dir.magnitude;
        dir.Normalize();

        if (!IsFleeing)
        {
            if (distance >= HitDistance)
            {
                transform.DOBlendableMoveBy(dir * Speed * Time.deltaTime, 0.01f);
            }
        }
        else
        {
            transform.DOBlendableMoveBy(dir * Speed * Time.deltaTime, 0.01f);
        }
    }

    private void Attack()
    {
        if (counter < HitCooldown)
        {
            return;
        }

        if (!HasTarget())
        {
            FindTarget();
        }

        Vector3 dir = AgroPlayer.transform.position - transform.position;
        float distance = dir.magnitude;

        if (distance < HitDistance && !IsFleeing)
        {
            transform.DOBlendableMoveBy(dir.normalized*0.5f, 0.2f).SetEase(Ease.OutExpo);
            AgroPlayer.GetComponent<Health>().Damage(Damage, false, this);
            gameObject.SetActive(true);
            state = State.Idle;
            counter = 0;
        }
    }
    
    public void Hit(Health health)
    {
        var hitFrom = (health.lastInflictedBy as Component).transform.position;
        transform.DOBlendableMoveBy((transform.position-hitFrom).normalized * 3f, 0.3f).SetEase(Ease.OutExpo);
    }

    public void Death(Health health)
    {   
        OnDeath();
    }
}
