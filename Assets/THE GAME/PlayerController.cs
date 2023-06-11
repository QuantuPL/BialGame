using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public bool IsDead = false;
    public int player;
    public UnityEvent OnPick;

    public Transform directional;
    public Swipe swipe;

    public AudioClip SlashClip;
    public AudioClip PunchClip;

    public Vector3 lastDir;

    private CraftingUI cui;

    private bool lockMovement = false;
    private bool movedUI = false;

    private AudioSource source;

    private void Start()
    {
        source = gameObject.AddComponent<AudioSource>();

        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, -3f);

        transform.GetChild(0).DORotate(new Vector3(0f, 0f, 6f), 0.3f)
            .SetLoops(-1, LoopType.Yoyo)  // Makes the rotation loop back and forth
            .SetEase(Ease.InOutQuad);   
            
        transform.GetChild(0).DOBlendableLocalMoveBy(new Vector3(0f, 0.1f, 0), 0.15f)
            .SetLoops(-1, LoopType.Yoyo)  // Makes the rotation loop back and forth
            .SetEase(Ease.InOutSine);
    }

    public void Update()
    {
        var movement = new Vector3(Input.GetAxis("HP" + PlayerModifier()), Input.GetAxis("VP" + PlayerModifier()));

        if (lockMovement)
        {
            movement = Vector3.zero;
        }

        if (movement.magnitude > 0.1f)
        {
            lastDir = movement;
        }

        //        directional.rotation = Quaternion.LookRotation(lastDir, Vector3.forward);

        transform.DOBlendableMoveBy(movement * Time.deltaTime * 5f, 0.01f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
        }

        if (!lockMovement)
        {
            HandleNormalInput();
        }
        else
        {
            HandleLegendaryInput();
        }
    }

    public void HandleNormalInput()
    {
        if (Input.GetButtonDown("PickP" + PlayerModifier()))
        {
            OnPick.Invoke();
        }
        if (Input.GetButtonDown("SlashP" + PlayerModifier()))
        {
            Attack();
        }

        if (Input.GetButtonDown("UseP" + PlayerModifier()))
        {
            Use();
        }
    }

    private void HandleLegendaryInput()
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

    private void Attack()
    {
        transform.DOBlendableMoveBy(lastDir.normalized * 0.5f, 0.2f).SetEase(Ease.OutExpo);
        //swipe.SwipeStart(); 
        var damaged = FindHealthInArc(transform.position, lastDir.normalized, 2f, 60f);
        var myHealth = GetComponent<Health>();
        foreach (var health in damaged)
        {
            if (health != myHealth)
            {
                health.Damage(1, false, this);
            }
        }

        if (damaged.Count > 1)
        {
            source.PlayOneShot(PunchClip, 1);
        }
        else
        {
            source.PlayOneShot(SlashClip, 1);
        }
    }

    public void Hit(Health health)
    {
        var hitFrom = (health.lastInflictedBy as Component).transform.position;
        transform.DOBlendableMoveBy((transform.position - hitFrom).normalized * 3f, 0.3f).SetEase(Ease.OutExpo);
    }

    public void Use()
    {
        var cs = FindObjectOfType<CraftingStation>();
        if (Vector3.Distance(cs.transform.position, transform.position) <= 1f)
        {
            cui = cs.StartUsing();
            lockMovement = cui != null;
        }
    }

    string PlayerModifier()
    {
        return player.ToString();
    }

    public static List<Health> FindHealthInArc(Vector3 position, Vector3 direction, float arcRadius, float arcAngle)
    {
        List<Health> healthComponents = new List<Health>();

        // Get all health components in the scene
        Health[] allHealths = GameObject.FindObjectsOfType<Health>();

        foreach (Health health in allHealths)
        {
            // Check if the health component is within the given radius
            Vector3 diff = health.transform.position - position;

            if (diff.magnitude <= arcRadius)
            {
                // Check if the health component is within the given angle
                float angle = Vector3.Angle(direction, diff);

                if (angle <= arcAngle * 0.5f)  // Because the angle is total, we only consider half for the left and right direction
                {
                    // If it's within radius and angle, add it to the list
                    healthComponents.Add(health);
                }
            }
        }

        return healthComponents;
    }
}
