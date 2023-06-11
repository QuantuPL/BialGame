using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TheGame : MonoBehaviour
{
    public float spawnRadius;

    public float HalfDayTime = 15;

    public VikingBoat boatPrefab;
    
    public List<Wave> waves;

    [Header("Animals")]
    public int AnimalSpawnCount = 3;
    public Transform AnimalAreaMin;
    public Transform AnimalAreaMax;
    public GameObject[] Animals;

    [Header("UI")]
    public TextMeshProUGUI DayCounter;
    public Image Clock;
    public Image NightFilter;

    [Header("Audio")]
    public AudioSource ChillMusic;
    public AudioSource WarMusic;
    public AudioClip HornClip;

    private bool isDay = false;
    public int currentDay = -1;

    public void Start()
    {
        WarMusic.volume = 0;
        WarMusic.Stop();
        ChillMusic.volume = 0;
        ChillMusic.Play();
        StartCoroutine(DayNightCycle());
    }

    private float counter = 0;
    void Update ()
    {
        counter += Time.deltaTime;
        float angle = counter / (HalfDayTime * 2) * 360 + 90;

        Clock.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void AnnounceDay()
    {
        currentDay++;
        var v = FindObjectsOfType<Viking>();
        foreach (var viking in v)
        {
            viking.IsFleeing = true;
            viking.state = Viking.State.ToBoat;
        }

        var b = FindObjectsOfType<VikingBoat>();
        foreach (var vikingBoat in b)
        {
            vikingBoat.state = VikingBoat.State.WaitingToFlee;
        }

        StartCoroutine(SpawnAnimals());

        NightFilter.gameObject.SetActive(false);
        DayCounter.text = currentDay.ToString();

        ChillMusic.DOFade(1, 1f);
        WarMusic.DOFade(0, 0.9f);

        isDay = true;
    }

    public void AnnounceNight()
    {
        var a = FindObjectsOfType<Animal>();
        foreach (var animal in a)
        {
            animal.Kill();
        }

        StartCoroutine(SpawnVikingBoats(waves[currentDay]));
        NightFilter.gameObject.SetActive(true);
        ChillMusic.DOFade(0, 0.4f);
        WarMusic.Stop();
        WarMusic.time = 0;
        WarMusic.Play();
        WarMusic.DOFade(1, 2);
        WarMusic.PlayOneShot(HornClip, 1);

        isDay = false;
    }

    IEnumerator DayNightCycle()
    {
        while (true)
        {
            if (isDay)
            {
                AnnounceNight();
            }
            else
            {
                AnnounceDay();
            }
            yield return new WaitForSeconds(HalfDayTime);    
        }
    }
    
    IEnumerator SpawnVikingBoats(Wave wave)
    {
        yield return new WaitForSeconds(0.3f);
        
        for (int i = 0; i < wave.boats.Count; i++)
        {
            Vector3 pos = Random.insideUnitCircle.normalized * spawnRadius;

            GameObject go = Instantiate(boatPrefab.gameObject, pos, Quaternion.identity);

            VikingBoat boat = go.GetComponent<VikingBoat>();
            Debug.Log(boat.name);
            foreach (var passenger in wave.boats[i].passengers)
            {
                boat.AddPayload(passenger); 
            }

            boat.ToBoattle();

            yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        }
    }

    IEnumerator SpawnAnimals ()
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < AnimalSpawnCount; i++)
        {
            GameObject a = Animals[Random.Range(0, Animals.Length)];

            GameObject go = Instantiate(a);

            Animal animal = go.GetComponent<Animal>();
            animal.AreaMin = AnimalAreaMin.position;
            animal.AreaMax = AnimalAreaMax.position;

            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
    }
    
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<Boat> boats;
}

[System.Serializable]
public struct Boat
{
    public string name;
    public List<Passenger> passengers;
}

[System.Serializable]
public struct Passenger
{
    public string name;
    public GameObject passenger;
    public GameObject rune;
}
