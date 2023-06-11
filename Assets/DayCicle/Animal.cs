using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public GameObject MeatPref;
    public float Speed = 2f;

    public float NearThreshold = 0.2f;

    public Vector2 AreaMin;
    public Vector2 AreaMax;

    private Vector3 TargetPos;


    void Start()
    {
        NewPos();
        transform.position = TargetPos;
        NewPos();

        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, -3f);

        transform.GetChild(0).DORotate(new Vector3(0f, 0f, 6f), 0.3f)
            .SetLoops(-1, LoopType.Yoyo)  // Makes the rotation loop back and forth
            .SetEase(Ease.InOutQuad);

        transform.GetChild(0).DOBlendableLocalMoveBy(new Vector3(0f, 0.1f, 0), 0.15f)
            .SetLoops(-1, LoopType.Yoyo)  // Makes the rotation loop back and forth
            .SetEase(Ease.InOutSine);

    }

    void Update()
    {
        Vector3 dir = TargetPos - transform.position;

        if (dir.magnitude < NearThreshold)
        {
            NewPos();
        }

        dir.Normalize();

        transform.DOBlendableMoveBy(dir * Speed * Time.deltaTime, 0.01f);
    }

    private void NewPos()
    {
        TargetPos = new Vector3(Random.Range(AreaMin.x, AreaMax.x), Random.Range(AreaMin.y, AreaMax.y));

        float dir = TargetPos.x - transform.position.x;

        dir = dir > 0 ? 1 : -1;

        transform.localScale = new Vector3(dir, 1, 1);
    }

    public void OnHit(Health h)
    {
        NewPos();
        var hitFrom = (h.lastInflictedBy as Component).transform.position;
        transform.DOBlendableMoveBy((transform.position - hitFrom).normalized * 3f, 0.3f).SetEase(Ease.OutExpo);
    }

    public void OnDeath(Health h)
    {
        Destroy(gameObject);

        GameObject go = Instantiate(MeatPref, transform.position, Quaternion.identity);
        go.transform.DoAnimateItem();
    }

    public void Kill ()
    {
        Destroy (gameObject);
    }
}
