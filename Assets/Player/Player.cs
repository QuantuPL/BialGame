using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isPlayer1 = true;

    public Transform holder;
    public Pickable holding;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("HP"+PlayerModifier()), Input.GetAxis("VP"+PlayerModifier()))*Time.deltaTime*5;
        if (Input.GetButtonDown("PickP"+PlayerModifier()))
        {
            HandlePick();
        }
        if (Input.GetButtonDown("UseP"+PlayerModifier()))
        {
            Debug.Log("Use Player "+PlayerModifier());
        }
        if (Input.GetButtonDown("SlashP"+PlayerModifier()))
        {
            Debug.Log("Slash Player "+PlayerModifier());
        }


    }

    void HandlePick()
    {
        var last = holding; // register last holding item
        if (IsCloseToAnyPickable(1f,last, out var best))
        {
            if (best.owner == null) //has no owner
            {
                if (HasItem(out _))
                {
                    Drop(best.transform.position);
                }
                Pick(best);
            }
            else //other player has it 
            {
                var otherPlayer = best.owner;
                otherPlayer.Drop(Vector3.zero);
                if (HasItem(out var item))
                {
                    Drop(Vector3.zero);
                    otherPlayer.Pick(item);
                }
                Pick(best);
            }
        }
        else
        {
            if (HasItem(out _))
            {
                //Drop current
                Drop(transform.position);
            }
        }
    }

    public bool HasItem(out Pickable item)
    {
        item = holding;
        return holding != null;
    }
    public void Drop(Vector3 targetPosition)
    {
        //Drop current
        holding.transform.parent = null;
        holding.transform.position = targetPosition;
        holding.owner = null;
        holding = null;
    }
    public void Pick(Pickable p)
    {
        //Pick new
        holding = p;
        holding.owner = this;
        holding.transform.parent = holder;
        holding.transform.localPosition = Vector3.zero;
    }

    string PlayerModifier()
    {
        return isPlayer1 ? "1" : "2";
    }

    public bool IsCloseToAnyPickable(float radius, Pickable last, out Pickable obj)
    {
        obj = null;
        var best = Pickable.all.Where(p => p != last).OrderBy(pickable => Vector3.Distance(pickable.transform.position, transform.position)).First();
        if (Vector3.Distance(
                best.transform.position, transform.position) <= radius)
        {
            obj = best;
            return true;
        }

        return false;
    }
}   