using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viking : MonoBehaviour
{
    public enum State { ToTreasury, ToPlayer, ToBoat, Idle }

    public float DropRunChance = 5;
    public GameObject UnknownRunePref;
    public State state;
    public bool HasGold = false;
    public bool IsRetrieve = false;
    public bool IsInBoat = false;

    public VikingBoat Boat;
    protected Vector3 destination;

    public Player AgroPlayer;

    public virtual void Start()
    {
        DayCycle.Instance.OnCycle.AddListener(NightEnds);
        GetComponent<Hitable>().OnDeath.AddListener(OnDeath);
    }

    private void NightEnds (bool isDay)
    {
        if (!isDay)
        {
            return;
        }

        state = State.ToBoat;
        IsRetrieve = true;
        destination = Boat.transform.position;
    }

    private void OnDeath ()
    {
        if (HasGold)
        {
            Instantiate (Treasury.Instance.GoldPref, transform.position, Quaternion.identity);
        }

        float val = Random.Range(0f, 100f);
        
        if (val < DropRunChance)
        {
            Instantiate (UnknownRunePref, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
