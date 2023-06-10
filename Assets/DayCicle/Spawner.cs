using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int AnimalsSpawnCount = 4;

    [Header("Animals")]
    public Transform AnimalAreaMin;
    public Transform AnimalAreaMax;
    public GameObject[] Animals;

    [Header("Vikings")]
    public float SpawnVikingsRate = 0.75f;

    public GameObject VikingBoatPref;
    public Transform[] VikingSpawnPoints;

    void Start()
    {
        DayCycle.Instance.OnCycle.AddListener(Cycle);
    }

    private void Cycle(bool isDay)
    {
        StopAllCoroutines();

        if (isDay)
        {
            StartCoroutine(SpawnAnimals());
        }
        else
        {
            StartCoroutine(SpawnVikings());
        }
    }

    IEnumerator SpawnAnimals()
    {
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < AnimalsSpawnCount; i++)
        {
            GameObject a = Animals[Random.Range(0, Animals.Length)];

            GameObject go = Instantiate(a);

            Animal animal = go.GetComponent<Animal>();
            animal.AreaMin = AnimalAreaMin.position;
            animal.AreaMax = AnimalAreaMax.position;

            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
    }

    IEnumerator SpawnVikings()
    {
        yield return new WaitForSeconds(0.3f);

        int vikingSpawnCount = Mathf.CeilToInt(0.75f * DayCycle.Instance.DayCount);

        for (int i = 0; i < vikingSpawnCount; i++)
        {
            Vector3 pos = VikingSpawnPoints[Random.Range(0, VikingSpawnPoints.Length)].position;

            GameObject go = Instantiate(VikingBoatPref, pos, Quaternion.identity);

            Animal animal = go.GetComponent<Animal>();
            animal.AreaMin = AnimalAreaMin.position;
            animal.AreaMax = AnimalAreaMax.position;

            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
    }
}
