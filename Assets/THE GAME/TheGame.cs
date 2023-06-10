using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TheGame : MonoBehaviour
{
    public float spawnRadius;

    public VikingBoat boatPrefab;
    
    public List<Wave> waves;

    private bool isDay = false;
    public int currentDay = -1;

    public void Start()
    {
        StartCoroutine(DayNightCycle());
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

        isDay = true;
    }

    public void AnnounceNight()
    {
        StartCoroutine(SpawnVikingBoats(waves[currentDay]));
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
            yield return new WaitForSeconds(15);    
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
