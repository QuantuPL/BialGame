using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hitable))]
public class Animal : MonoBehaviour
{
    public float Speed = 2f;

    public float NearThreshold = 0.2f;

    public Vector3 AreaMin;
    public Vector3 AreaMax;

    private Vector3 TargetPos;

    void Start()
    {
        GetComponent<Hitable>().OnHit.AddListener(OnHit);
        DayCycle.Instance.OnCycle.AddListener(OnCycle);

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
}
