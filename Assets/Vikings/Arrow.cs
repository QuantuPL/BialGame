using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float Speed = 5;
    public int Damage = 2;
    public float DestroyDelay = 3f;
    public float GracePeriod = 0.3f;

    private float counter;
    void Start()
    {
        Destroy (gameObject, DestroyDelay);
    }

    void Update()
    {
        counter += Time.deltaTime;
        transform.Translate (transform.up * Speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (GracePeriod > counter) {
            return;
        }
/*

        if (!h) {
            return;
        }
        

        h.Hit(2, player);
*/
        Destroy (gameObject);
    }
}
