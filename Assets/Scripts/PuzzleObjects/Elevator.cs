using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float maxHeight;
    public bool isOnTop;
    [SerializeField] float speed = 1;

    Transform metalPlatform;
    AudioSource elevatorSound;
    bool isMoving = false;
    bool direction = false; //false: down, true: up

    void Awake()
    {
        metalPlatform = transform.GetChild(0);
        elevatorSound = GetComponent<AudioSource>();

        if (isOnTop) MovePlatform(true);
    }

    void Update()
    {

        //Hareket
        if (isMoving)
        {
            if (direction)
            {
                if(metalPlatform.localPosition.y < maxHeight)
                {
                    //Yukarı çık
                    metalPlatform.position += transform.up * Time.deltaTime * speed;
                }
                else
                {
                    //Durdur
                    isMoving = false;
                    metalPlatform.localPosition = new Vector3(0, maxHeight, 0);
                    elevatorSound.Stop();
                }
            }
            else
            {
                if (metalPlatform.localPosition.y > 0)
                {
                    //Aşağı çık
                    metalPlatform.position -= transform.up * Time.deltaTime * speed;
                }
                else
                {
                    //Durdur
                    isMoving = false;
                    metalPlatform.localPosition = new Vector3(0, 0, 0);
                    elevatorSound.Stop();
                }
            }
        }
    }

    public void MovePlatform(bool toUp)
    {
        direction = toUp;
        isMoving = true;

        elevatorSound.Play();
    }
}
