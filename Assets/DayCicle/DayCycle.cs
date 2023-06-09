using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayCycle : MonoBehaviour
{
    public static DayCycle Instance;

    public float NightTime;
    public float DayTime;
    public bool IsDay = true;

    public UnityEvent<bool> OnCycle;

    private float counter;

    void Awake ()
    {
        Instance = this;
    }

    void Start()
    {
        OnCycle.AddListener(TimeChanged);
    }

    void Update()
    {
        counter += Time.deltaTime;

        float t = IsDay ? DayTime : NightTime;

        if (counter > t)
        {
            counter -= t;

            IsDay = !IsDay;
            OnCycle.Invoke(IsDay);
        }
    }

    void TimeChanged (bool day)
    {

    }
}
