using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Picking : MonoBehaviour, IPickableOwner
{
    public Pickable Item { get; set; }
    
    public void Pick()
    {
        var last = Item; // register last holding item
        if (IsCloseToAnyPickable(1f, last, out var best))
        {
            if (best.Owner == null) //has no owner
            {
                if (HasItem(out _))
                {
                    Drop(best.transform.position);
                }
                Take(best);
            }
            else //other owner has it 
            {
                var otherPlayer = best.Owner;
                otherPlayer.Drop(Vector3.zero);
                if (HasItem(out var item))
                {
                    Drop(Vector3.zero);
                    otherPlayer.Take(item);
                }
                Take(best);
            }
        }
        else
        {
            if (HasItem(out var item))
            {
                //Drop current
                Drop(transform.position);
                var ClosePlaceSpot = FindObjectsOfType<Picking>().
                    Where(p => p != last && p != this)?.OrderBy(pickable => Vector3.Distance(pickable.transform.position, transform.position)).First();
                if (Vector3.Distance(
                        ClosePlaceSpot.transform.position, transform.position) <= 1f)
                {
                    ClosePlaceSpot.Take(item);
                }
            }
        }
    }
    
    public bool IsCloseToAnyPickable(float radius, Pickable last, out Pickable obj)
    {
        obj = null;
        if (Pickable.all.Count == 0 || Pickable.all.TrueForAll(x => x == last)) return false;
        var best = Pickable.all.Where(p => p != last).OrderBy(pickable => Vector3.Distance(pickable.transform.position, transform.position)).First();
        if (Vector3.Distance(
                best.transform.position, transform.position) <= radius)
        {
            obj = best;
            return true;
        }

        return false;
    }
    
    public bool HasItem(out Pickable item)
    {
        item = Item;
        return Item != null;
    }
    public void Drop(Vector3 targetPosition)
    {
        //Drop current
        Item.transform.parent = null;
        Item.transform.position = targetPosition;
        Item.Owner = null;
        Item = null;
    }
    public void Take(Pickable p)
    {
        //Pick new
        Item = p;
        Item.Owner = this;
        Item.transform.parent = transform;
        Item.transform.localPosition = Vector3.zero;
    }
}
