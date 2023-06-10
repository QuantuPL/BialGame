using DG.Tweening;
using UnityEngine;

public class GoldSnatcher : Viking
{
    public float Speed = 2;
    public float SpeedWithGold = 1.5f;
    public bool HasGold;

    public Transform GoldInHands;
    public Pickable goldPrefab;

    public override void Start()
    {
        base.Start();

        state = State.ToTreasury;

        destination = Treasury.Instance.transform.position;

        GoldInHands.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        
        var speed = HasGold ? SpeedWithGold : Speed;
        var dir = destination - transform.position;
        var dist = dir.magnitude;
        dir.Normalize();

        transform.DOBlendableMoveBy(dir * speed * Time.deltaTime, 0.01f);

        if (dist < 0.4f)
        {
            if (state == State.ToTreasury)
            {
                Treasury.Instance.TakeGold();
                HasGold = true;
                GoldInHands.gameObject.SetActive(true);

                destination = Boat.transform.position;
                state = State.ToBoat;
            }
            else
            {
                if (HasGold)
                {
                    HasGold = false;
                    GoldInHands.gameObject.SetActive(false);

                    Treasury.Instance.LostGold();
                }

                if (!IsFleeing)
                {
                    destination = Treasury.Instance.transform.position;
                    state = State.ToTreasury;
                } else
                {
                    IsInBoat = true;
                    gameObject.SetActive(false);
                    state = State.Idle;
                }
            }
        }
    }
    
    public void Hit(Health health)
    {
        var hitFrom = (health.lastInflictedBy as Component).transform.position;
        transform.DOBlendableMoveBy((transform.position-hitFrom).normalized * 3f, 0.3f).SetEase(Ease.OutExpo);
    }

    public void Death(Health health)
    {
        if (HasGold)
        {
            var gold = Instantiate(goldPrefab);
            gold.transform.position = transform.position;
        }

        OnDeath();
    }
}
