using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IPickableOwner
{
    public static List<Player> PlayerList = new List<Player>();

    public int StartHealth = 3;
    public bool IsDead = false;
    public float ReviveTime = 5;
    public bool isPlayer1 = true;
    public int HandDmg = 10;
    public Transform holder;
    public Pickable holding;

    public int layerMask;

    void Start ()
    {
        PlayerList.Add(this);
        layerMask = LayerMask.GetMask("Hitable");
    }

    void OnDestroy()
    {
        PlayerList.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockMovement)
        {
            if (new Vector3(Input.GetAxis("HP" + PlayerModifier()), Input.GetAxis("VP" + PlayerModifier())).magnitude >
                0.1f)
            {
                lastDir = new Vector3(Input.GetAxis("HP" + PlayerModifier()), Input.GetAxis("VP" + PlayerModifier()));
            }

            transform.position +=
                new Vector3(Input.GetAxis("HP" + PlayerModifier()), Input.GetAxis("VP" + PlayerModifier())) *
                Time.deltaTime * 5;
            if (Input.GetButtonDown("PickP" + PlayerModifier()))
            {
                HandlePick();
            }

            if (Input.GetButtonDown("UseP" + PlayerModifier()))
            {
                var cs = FindObjectOfType<CraftingStation>();
                if (Vector3.Distance(cs.transform.position, transform.position)<= 1f)
                {
                    cui = cs.StartUsing();
                    lockMovement = cui!=null;
                }
            }

            if (Input.GetButtonDown("SlashP" + PlayerModifier()))
            {
                Hit();
            }
        }
        else
        {
            if (Input.GetButtonDown("SlashP" + PlayerModifier()))
            {
                var cs = FindObjectOfType<CraftingStation>();
                cs.StopUsing();
                cui = null;
                lockMovement = false;
            }
            if (Input.GetButtonDown("UseP" + PlayerModifier()))
            {
                var cs = FindObjectOfType<CraftingStation>();
                cs.Craft();
                cs.StopUsing();
                lockMovement = false;
            }
            if (Input.GetAxisRaw("HP" + PlayerModifier()) != 0f)
            {
                if (!movedUI)
                {
                    if (Input.GetAxisRaw("HP" + PlayerModifier()) < 0f)
                    {
                        cui.MoveLast();
                    }
                    else
                    {
                        cui.MoveNext();
                    }

                    movedUI = true;
                }
            }
            else
            {
                movedUI = false;
            }
        }

    }

    void HandlePick()
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
                Pick(best);
            }
            else //other owner has it 
            {
                var otherPlayer = best.Owner;
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
            if (HasItem(out var item))
            {
                //Drop current
                Drop(transform.position);
                var ClosePlaceSpot = FindObjectsOfType<PlaceSpot>().
                    Where(p => p != last).OrderBy(pickable => Vector3.Distance(pickable.holder.position, transform.position)).First();
                if (Vector3.Distance(
                        ClosePlaceSpot.transform.position, transform.position) <= 1f)
                {
                    ClosePlaceSpot.Pick(item);
                }
            }
        }
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
    public void Pick(Pickable p)
    {
        //Pick new
        Item = p;
        Item.Owner = this;
        Item.transform.parent = holder;
        Item.transform.localPosition = Vector3.zero;
    }

    string PlayerModifier()
    {
        return isPlayer1 ? "1" : "2";
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

    private void Hit()
    {
        //TODO animacje

        Vector2 castPos = transform.position;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(castPos, 1.2f, Vector2.zero, 0.01f, layerMask);
        print(hits.Length);
        if (hits.Length == 0)
        {
            return;
        }

        int index = -1;
        float dist = float.PositiveInfinity;
        for (int i = 0; i < hits.Length; i++)
        {
            float d = ((Vector2)hits[i].transform.position - castPos).magnitude;

            if (dist > d)
            {
                index = i;
                dist = d;
            }
        }

        GameObject go = hits[index].collider.gameObject;

        Hitable h = go.GetComponentInParent<Hitable>();

        if (!h)
        {
            return;
        }

        //TODO calc dmg from item
        int dmg = HandDmg;

        if (HasItem(out var item) && item is Tool tool)
        {
            if (tool.effectiveAgainst == h.Type)
            {
                dmg += tool.damageBoost;
            }            
        }

        h.Hit(dmg);
    }

    public Pickable Item { get; set; }
}
