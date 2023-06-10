using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private static Spawner instance;

    public int AnimalsSpawnCount = 4;

    [Header("Animals")]
    public Transform AnimalAreaMin;
    public Transform AnimalAreaMax;
    public GameObject[] Animals;

    [Header("Vikings")]
    public float SpawnVikingsRate = 0.75f;

    public GameObject VikingBoatPref;
    public Transform[] VikingSpawnPoints;

    public int GoldSnatchersDay = 3;
    public GameObject[] Vikings;

    private void Awake()
    {
        instance = this;
    }

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
            StartCoroutine(SpawnVikingBoats());
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

    IEnumerator SpawnVikingBoats()
    {
        yield return new WaitForSeconds(0.3f);

        int vikingSpawnCount = Mathf.CeilToInt(0.75f * DayCycle.DayCount);

        for (int i = 0; i < vikingSpawnCount; i++)
        {
            Vector3 pos = VikingSpawnPoints[Random.Range(0, VikingSpawnPoints.Length)].position;

            GameObject go = Instantiate(VikingBoatPref, pos, Quaternion.identity);

            VikingBoat boat = go.GetComponent<VikingBoat>();


            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
    }

    public static void SpawnVikings(VikingBoat boat, int count)
    {
        instance.SpawnViks(boat, count);
    }

    private void SpawnViks(VikingBoat boat, int count)
    {

        int vIndex = 0;

        for (int i = 0; i < count; i++)
        {

            GameObject go = Instantiate(Vikings[vIndex], boat.transform.position, Quaternion.identity);

            Viking v = go.GetComponent<Viking>();

            v.Boat = boat;
            boat.AddViking(v);

            if (DayCycle.DayCount > GoldSnatchersDay)
            {
                vIndex = Random.Range(0, Vikings.Length);
            }
            else
            {
                vIndex = Random.Range(1, Vikings.Length);
            }
        }
    }
}
