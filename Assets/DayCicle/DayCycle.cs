using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayCycle : MonoBehaviour
{
    public static DayCycle Instance;

    public static int DayCount = 1;

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
        DayCount = 1;
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

    private void TimeChanged (bool day)
    {
        if (day)
        {
            DayCount++;
        }
    }

}
