using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isDoorOpen = false;

    float currentDoorPos = 0;
    Animator animator;
    BoxCollider col;
    Transform door1;
    Transform door2;

    void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider>();

        animator.SetBool("openState", isDoorOpen);
        col.enabled = !isDoorOpen;
    }

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("openState", isDoorOpen);
    }

    public void ChangeDoorState(bool isOpen)
    {
        col.enabled = !isOpen;
        if(isDoorOpen != isOpen) GetComponent<AudioSource>().Play();
        animator.SetBool("openState", isOpen);

        isDoorOpen = isOpen;
    }
}
