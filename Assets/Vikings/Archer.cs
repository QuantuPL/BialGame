using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Archer : Viking
{
    public float NearDistance = 4f;
    public float FarDistnace = 5f;
    public float HitCooldown = 2f;

    public float Speed = 4;

    private float counter = 0;

    public GameObject ArrowPref;

    private bool isMoving = true;

    public override void Start()
    {
        base.Start();
        state = State.ToPlayer;
        counter = 10000;
    }

    void Update()
    {
        counter += Time.deltaTime;

        Movement();

        Attack();
    }

    private void Movement()
    {
        Player currentPlayer = AgroPlayer;

        if (!currentPlayer)
        {
            var list = Player.PlayerList;
            int index = 0;
            float dist = float.MaxValue;

            for (int i = 0; i < list.Count; i++)
            {
                float d = (list[i].transform.position - transform.position).magnitude;

                if (d < dist)
                {
                    dist = d;
                    index = i;
                }
            }

            currentPlayer = list[index];
        }

        Vector3 destination = IsRetrieve ? Boat.transform.position : currentPlayer.transform.position;

        Vector3 dir = destination - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();


        if (IsRetrieve && distance < 0.3f)
        {
            IsInBoat = true;
            gameObject.SetActive(true);
            state = State.Idle;
            counter = -1000;
            return;
        }

        if (isMoving)
        {
            if (distance < NearDistance)
            {
                isMoving = false;
            }
            transform.Translate(dir * Speed * Time.deltaTime);
        }
        else if (distance > FarDistnace)
        {
            isMoving = true;
        }
    }

    private void Attack()
    {
        if (counter < HitCooldown)
        {
            return;
        }

        Player currentPlayer = AgroPlayer;

        if (!currentPlayer)
        {
            var list = Player.PlayerList;
            int index = 0;
            float dist = float.MaxValue;

            for (int i = 0; i < list.Count; i++)
            {
                float d = (list[i].transform.position - transform.position).magnitude;

                if (d < dist)
                {
                    dist = d;
                    index = i;
                }
            }

            currentPlayer = list[index];
        }

        Vector3 dir = currentPlayer.transform.position - transform.position;
        float distance = dir.magnitude;
        dir.Normalize();

        if (!IsRetrieve)
        {

            Instantiate (ArrowPref, transform.position, Quaternion.LookRotation(Vector3.forward, dir));

            gameObject.SetActive(true);
            state = State.Idle;
            counter = 0;
        }
    }
}
*/  