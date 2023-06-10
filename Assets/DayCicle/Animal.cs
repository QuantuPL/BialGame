using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public float Speed = 2f;

    public float NearThreshold = 0.2f;

    public Vector2 AreaMin;
    public Vector2 AreaMax;

    private Vector3 TargetPos;

    void Start()
    {
        DayCycle.Instance.OnCycle.AddListener(OnCycle);

        NewPos();
        transform.position = TargetPos;
        NewPos();
    }

    void Update()
    {
        Vector3 dir = TargetPos - transform.position;

        if (dir.magnitude < NearThreshold)
        {
            NewPos();
        }

        dir.Normalize();

        transform.Translate(dir * Time.deltaTime);
    }

    private void NewPos()
    {
        TargetPos = new Vector3(Random.Range(AreaMin.x, AreaMax.x), Random.Range(AreaMin.y, AreaMax.y));

        float dir = TargetPos.x - transform.position.x;

        dir = dir > 0 ? 1 : -1;

        transform.localScale = new Vector3(dir, 1, 1);
    }

    private void OnHit()
    {
        NewPos();
    }

    private void OnCycle(bool isDay)
    {
        if (isDay)
        {
            return;
        }

        Destroy(gameObject);
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
