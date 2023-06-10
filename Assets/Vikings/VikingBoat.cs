using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingBoat : MonoBehaviour
{
    public enum State { ToIsland, Idle, Retrieve, FromIsland }

    public State state = State.ToIsland;
    public float Speed = 3f;
    public int VikingCount = 1;
    private List<Viking> vikings;

    private Vector3 startPoint;
    private Vector3 destination;

    void Start()
    {
        startPoint = transform.position;
        destination = Treasury.pos;

        vikings = new List<Viking>();

        DayCycle.Instance.OnCycle.AddListener(RetrieveStart);
    }

    private void Update()
    {
        Movement();

        if (state == State.FromIsland)
        {
            Vector3 dist = destination - transform.position;

            if (dist.magnitude < 0.3f)
            {
                Destroy(gameObject);
            }
        }

        if (state == State.Retrieve)
        {
            bool isAnyOnIsland = false;

            for (int i = 0; i < VikingCount; i++)
            {
                Viking v = vikings[i];
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
                destination = startPoint;
            }
        }
    }

    private void Movement()
    {
        switch (state)
        {
            case State.ToIsland:
            case State.FromIsland:

                Vector3 dir = destination - transform.position;
                dir.Normalize();

                transform.Translate(Speed * Time.deltaTime * dir);

                float side = dir.x > 0 ? 1 : -1;
                transform.localScale = new Vector3(side, 1, 1);

                break;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("island reached");
        if (state == State.ToIsland)
        {
            state = State.Idle;

            SpawnVikings();
        }


    }

    private void SpawnVikings()
    {
        Spawner.SpawnVikings(this, VikingCount);
    }

    private void RetrieveStart(bool isDay)
    {
        if (!isDay)
        {
            return;
        }

        state = State.Retrieve;
    }

    public void AddViking (Viking v)
    {
        vikings.Add(v);
    }
}
