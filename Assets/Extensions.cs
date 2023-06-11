using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public static class Extensions
{
    public static void DoAnimateItem (this Transform t)
    {

        t.DOBlendableMoveBy(Vector3.down * 0.4f, 1.5f).SetEase(Ease.OutBounce);
        t.DOBlendableMoveBy(Vector3.right * Random.Range(-1, 1), 1.5f).SetEase(Ease.OutCubic);
    }
}
