using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickable : MonoBehaviour
{
    public static List<Pickable> all = new List<Pickable>();

    public void OnEnable()
    {
        all.Add(this);
    }

    public void OnDisable()
    {
        all.Remove(this);
    }

    public IPickableOwner Owner { get; set; }
}

public interface IPickableOwner
{
    Pickable Item { get; set; }

    public bool HasItem(out Pickable item);
    public void Drop(Vector3 targetPosition);
    public void Take(Pickable p);
}
