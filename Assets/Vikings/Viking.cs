using System;
using DG.Tweening;
using UnityEngine;

public class Viking : MonoBehaviour
{
    public enum State { ToTreasury, ToPlayer, ToBoat, Idle }

    public GameObject UnknownRunePref;
    public State state;
    public bool IsFleeing = false;
    public bool IsInBoat = false;

    public VikingBoat Boat;
    protected Vector3 destination;

    public PlayerController AgroPlayer;

    public virtual void Start()
    {
        //DayCycle.Instance.OnCycle.AddListener(NightEnds);
    }

    private void NightEnds (bool isDay)
    {
        if (!isDay)
        {
            return;
        }

        state = State.ToBoat;
        IsFleeing = true;
        destination = Boat.transform.position;
    }

    protected virtual void Update()
    {
        var allVikings = FindObjectsOfType<Viking>();
        for (int i = 0; i < 8; i++)
        {
            foreach (var viking in allVikings)
            {
                if (viking != this)
                {
                    var dist = Vector3.Distance(transform.position, viking.transform.position);

                    if (dist < 0.7f)
                    {
                        var dir = (transform.position - viking.transform.position).normalized * (0.7f - dist) / 2f;
                        dir = Quaternion.Euler(0, 0, 15) * dir / 8f;
                        this.transform.DOBlendableMoveBy(dir, 0);
                        viking.transform.DOBlendableMoveBy(-dir, 0);
                    }
                }
            }
        }
    }

    protected void OnDeath ()
    {
        if (UnknownRunePref)
        {
            Instantiate (UnknownRunePref, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public Viking WithRune(GameObject objRune)
    {
        UnknownRunePref = objRune;
        return this;
    }
}
