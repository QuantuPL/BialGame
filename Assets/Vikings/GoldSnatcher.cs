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
        
        destination = IsFleeing || HasGold ? Boat.transform.position : Treasury.Instance.transform.position;
        
        var speed = HasGold ? SpeedWithGold : Speed;
        var dir = destination - transform.position;
        var dist = dir.magnitude;
        dir.Normalize();

        if (!IsFleeing)
        {
            if (dist >= 0.4f)
            {
                transform.DOBlendableMoveBy(dir * speed * Time.deltaTime, 0);
            }
            else
            {
                if (HasGold)
                {
                    HasGold = false;
                    GoldInHands.gameObject.SetActive(false);
                    Treasury.Instance.LostGold();
                }
                else
                {
                    Treasury.Instance.TakeGold();
                    HasGold = true;
                    GoldInHands.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (dist >= 0.7f)
            {
                transform.DOBlendableMoveBy(dir * speed * Time.deltaTime, 0);
            }
            else
            {
                if (state != State.Idle)
                {
                    state = State.Idle;

                    transform.DOBlendableMoveBy(dir * dist, 0.5f);
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
