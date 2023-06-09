using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Pickable : MonoBehaviour
{
    public static List<Pickable> all = new List<Pickable>();
    [FormerlySerializedAs("holder")] public Player owner;

    public void OnEnable()
    {
        all.Add(this);
    }

    public void OnDisable()
    {
        all.Remove(this);
    }
}
