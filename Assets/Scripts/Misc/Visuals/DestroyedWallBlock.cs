using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedWallBlock : MonoBehaviour
{
    [SerializeField] GameObject shatterEffect;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.back * Random.Range(10f, 30f) + Vector3.up * Random.Range(0f, 20f);
        rb.AddTorque(Random.Range(-50f, 50f), Random.Range(-50f, 50f), Random.Range(-50f, 50f));

        Destroy(gameObject, 10);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.GetComponent<DestroyedWallBlock>())
        {
            GameObject eff = Instantiate(shatterEffect, transform.position, Quaternion.identity);
            Destroy(eff, 1.2f);
            Destroy(gameObject);
        }
    }
}
