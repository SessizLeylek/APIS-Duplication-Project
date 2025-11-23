using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWakeUp : MonoBehaviour
{
    public CharacterControlling controller;
    public Transform camTransform;

    Animator animator;
    Vector3 startPos;
    Quaternion startRot;

    private void Start()
    {
        if(PlayerPrefs.GetInt("progress") % 2 == 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            animator = GetComponent<Animator>();

            startPos = camTransform.position;
            startRot = camTransform.rotation;

            controller.movement.canMove = false;
            controller.camera.SetCameraFreedom(true);
        }
    }

    private void LateUpdate()
    {
        camTransform.position = transform.position;
        camTransform.rotation = transform.rotation;
    }

    public void EndAnimation()
    {
        camTransform.position = startPos;
        camTransform.rotation = startRot;

        controller.movement.canMove = true;
        controller.camera.SetCameraFreedom(false);
        gameObject.SetActive(false);
    }
}
