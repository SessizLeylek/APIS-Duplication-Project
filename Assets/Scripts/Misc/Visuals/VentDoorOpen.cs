using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentDoorOpen : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = GameObject.Find("MainPlayer").transform;
    }

    private void Update()
    {
        if(Vector3.Distance(player.position, transform.position) < 1.5f)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Destroy(this);
        }
    }
}
