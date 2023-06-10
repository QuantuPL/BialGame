using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class DayCycle : MonoBehaviour
{
    public static DayCycle Instance;

    public static int DayCount = 1;

    public float NightTime;
    public float DayTime;
    public bool IsDay = true;

    public UnityEvent<bool> OnCycle;

    public Image Clock;
    public TextMeshProUGUI DayCounter;

    private float counter;
    private float secondCounter;
    void Awake ()
    {
        Instance = this;
    }

    void Start()
    {
        DayCount = 1;
        DayCounter.text = DayCount.ToString();
        OnCycle.AddListener(TimeChanged);
    }

    void Update()
    {
        counter += Time.deltaTime;
        secondCounter += Time.deltaTime;

        float angle = secondCounter / (NightTime + DayTime) * 360;

        Clock.transform.rotation = Quaternion.Euler(0, 0, angle);

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
            DayCounter.text = DayCount.ToString();

            secondCounter -= NightTime;
            secondCounter -= DayTime;
        }
    }

}
