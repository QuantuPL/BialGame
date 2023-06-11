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
        int index = -1;
        float dist = float.MaxValue;

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].IsDead)
            {
                continue;
            }

            float d = (list[i].transform.position - transform.position).magnitude;

            if (d < dist)
            {
                dist = d;
                index = i;
            }
        }

        if (index < 0)
        {
            AgroPlayer = null;
        }
        else
        {
            AgroPlayer = list[index];
        }
    }

    private void Movement()
    {
        if (!HasTarget())
        {
            FindTarget();
        }

        var destination = IsFleeing ? Boat.transform.position : (!AgroPlayer? transform.position : AgroPlayer.transform.position);

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
            
            if (distance < 0.7f)
            {
                if (state != State.Idle)
                {
                    state = State.Idle;

                    transform.DOBlendableMoveBy(dir * distance, 0.5f);
                    transform.GetChild(0).DOLocalJump(Vector3.zero, Mathf.Abs(dir.x), 1, 0.5f).SetEase(Ease.InOutSine);
                    transform.GetChild(0).DOPunchScale(Vector3.one * Mathf.Abs(dir.y) * 0.3f, 0.5f, 0, 0)
                        .SetEase(Ease.InOutSine).OnComplete(
                            () =>
                            {
                                gameObject.SetActive(false);
                                gameObject.transform.parent = Boat.transform;
                                IsInBoat = true;

                            });
                }
            }
            else
            {
                transform.DOBlendableMoveBy(dir * Speed * Time.deltaTime, 0.01f);
            }
        }
    }

    private void Attack()
    {
        if (counter < HitCooldown)
        {
            return;
        }

        if (!HasTarget() || AgroPlayer.IsDead)
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
