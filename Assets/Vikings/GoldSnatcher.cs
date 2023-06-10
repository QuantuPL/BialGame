using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldSnatcher : Viking
{
    public float Speed = 2;
    public float SpeedWithGold = 1.5f;

    public Transform GoldInHands;

    public override void Start()
    {
        base.Start();

        state = State.ToTreasury;

        destination = Treasury.Instance.transform.position;

        GoldInHands.gameObject.SetActive(false);
    }

    void Update()
    {
        float speed = HasGold ? SpeedWithGold : Speed;

        Vector3 dir = destination - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        transform.Translate(dir * speed * Time.deltaTime);

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

                if (!IsRetrieve)
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
}
