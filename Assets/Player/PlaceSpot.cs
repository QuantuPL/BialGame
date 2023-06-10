using UnityEngine;

public class PlaceSpot : MonoBehaviour, IPickableOwner
{
    public Pickable Item { get; set; }
    public Transform holder;
    
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

    public void Pick(Pickable p)
    {
        //Pick new
        Item = p;
        Item.Owner = this;
        Item.transform.parent = holder;
        Item.transform.localPosition = Vector3.zero;
    }
}
