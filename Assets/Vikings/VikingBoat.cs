using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VikingBoat : MonoBehaviour
{
    public enum State { Idle, ToIsland, AtIsland, WaitingToFlee, FromIsland }

    public State state = State.Idle;
    public float Speed = 3f;
    public int VikingCount = 1;
    public List<Passenger> payload;

    private Vector3 startPoint;
    private Vector3 treasury;

    private List<GameObject> live;

    void Awake()
    {
        startPoint = transform.position;
        treasury = Treasury.pos;

        payload = new List<Passenger>();

        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, -1f);

        transform.GetChild(0).DORotate(new Vector3(0f, 0f, 2f), 2f)
            .SetLoops(-1, LoopType.Yoyo)  // Makes the rotation loop back and forth
            .SetEase(Ease.InOutQuad);
    }

    public void AddPayload(Passenger passenger)
    {
        payload.Add(passenger);
    }

    public void ToBoattle()
    {
        state = State.ToIsland;
    }
    
    private void Update()
    {
        if (state == State.ToIsland)
        {
            MoveTowardsIsland();
        }
        else if (state == State.AtIsland)
        {
            StartCoroutine(Dropoff());
            state = State.Idle;
        }
        else if (state == State.WaitingToFlee)
        {
            bool isAnyOnIsland = false;

            for (int i = 0; i < live.Count; i++)
            {
                if(!live[i])
                    continue;
                Viking v = live[i].GetComponent<Viking>();
                if (!v)
                {
                    continue;
                }

                if (!v.IsInBoat)
                {
                    isAnyOnIsland = true;
                }
            }

            if (!isAnyOnIsland)
            {
                state = State.FromIsland;
            }
        }
        else if (state == State.FromIsland)
        {
            MoveAwayFromIsland();

            if (Vector3.Distance(transform.position, startPoint) < 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    private void MoveTowardsIsland()
    {
        Vector3 dir = treasury - startPoint;
        dir.Normalize();

        transform.Translate(Speed * Time.deltaTime * dir);

        float side = dir.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(side, 1, 1);
    }

    private void MoveAwayFromIsland()
    {
        Vector3 dir = startPoint - treasury;
        dir.Normalize();

        transform.Translate(Speed * Time.deltaTime * dir);

        float side = dir.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(side, 1, 1);
    }

    private IEnumerator Dropoff()
    {
        live = new List<GameObject>();
        foreach (var viking in payload)
        {
            var v = Instantiate(viking.passenger.gameObject);
            v.GetComponent<Viking>().WithRune(viking.rune);
            live.Add(v);
            v.transform.position = transform.position;
            var disembarkDirection = (treasury - startPoint).normalized;
            v.transform.DOBlendableMoveBy(disembarkDirection, 0.5f);
            v.transform.GetChild(0).DOLocalJump(Vector3.zero, Mathf.Abs(disembarkDirection.x), 1, 0.5f).SetEase(Ease.InOutSine);
            v.transform.GetChild(0).DOPunchScale(Vector3.one* Mathf.Abs(disembarkDirection.y)*0.3f, 0.5f, 0, 0).SetEase(Ease.InOutSine);
            v.GetComponent<Viking>().Boat = this;
            yield return new WaitForSeconds(0.3f);
        }
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        print("island reached");
        if (state == State.ToIsland)
        {
            state = State.AtIsland;
        }


    }

}
