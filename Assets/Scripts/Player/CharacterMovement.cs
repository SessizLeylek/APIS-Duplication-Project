using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterMovement
{
    public CharacterController controller;

    public float playerSpeed = 4;
    public float jumpHeight = 5;
    public bool canMove = true;
    public LayerMask groundLayer;

    [HideInInspector] public Vector3 movementVector = Vector3.zero;
    [HideInInspector] public bool isJumping = false;
    float groundedDuration = 0; //Kaç saniyedir yerde duruyor
    float airDuration = 0;  //Karakterin kaç saniyedir havada kaldığı
    float fallSpeed = 0;    //Yerçekimi sırasında karakterin düşüş hızı
    bool isGrounded = false;    //Yerde mi kontrol
    Transform groundObject = null; //En son temas edilen yerdeki obje
    Vector3 lastPos;    //En son temas edilen objenin önceki pozisyonu
    [HideInInspector] public float stepCycleDelta;   //En son atılan adımdan beri gidilen mesafe (sesler için)
    [HideInInspector] public bool gravity = true;   //Yer çekiminden etkileinr mi

    //Karakterin hareket işlevi
    public void MovementCycle(Transform transform)
    {
        //WASD ile hareket
        float moveX = 0;
        float moveZ = 0;
        if (canMove)
        {
            moveX = Input.GetAxis("Horizontal");
            moveZ = Input.GetAxis("Vertical");

            Vector3 newMovementVector = ((transform.forward * moveZ) + (transform.right * moveX)) * (Input.GetKey(KeyCode.LeftShift) ? 2 : 1);
            movementVector = newMovementVector * Time.deltaTime * 5 + movementVector * (1 - Time.deltaTime * 5);
            
            Vector3 clampedMovVector = movementVector.magnitude > 0.1f ? movementVector : Vector3.zero;
            controller.Move(clampedMovVector * playerSpeed * Time.deltaTime);
        }

        //Yerçekimi
        isGrounded = false;
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position - Vector3.up * 0.6f, 0.4f, groundLayer);
        Transform overlappedTransform = null;
        foreach (Collider collider in overlappedColliders)
        {
            if (!overlappedTransform && !collider.isTrigger)
            {
                overlappedTransform = collider.transform;
                isGrounded = true;
            }
        }

        if (isGrounded)
        {
            //Sesler
            if (moveX != 0 || moveZ != 0) stepCycleDelta += Time.deltaTime * (Input.GetKey(KeyCode.LeftShift) ? 3 : 1.5f);

            if (stepCycleDelta > 1) //Yürüdükçe ses çıkarma
            {
                stepCycleDelta--;
                if (overlappedTransform.GetComponent<GroundType>())
                    controller.GetComponent<CharacterSoundSystem>().PlaySound(overlappedTransform.GetComponent<GroundType>().type, 1);
            }

            if(groundObject == null && airDuration > 0.2f)    //Yere ilk iniş
            {
                if (overlappedTransform.GetComponent<GroundType>())
                    controller.GetComponent<CharacterSoundSystem>().PlaySound(overlappedTransform.GetComponent<GroundType>().type, 1);
            }

            //Zemine bağlı kalma
            StickToTheGround(overlappedTransform);

            //Zıplama
            isJumping = false;
            fallSpeed = 0;
            if (Input.GetKey(KeyCode.Space) && canMove)
            {
                fallSpeed = jumpHeight;
                isJumping = true;
            }

            airDuration = 0;
        }
        else
        {
            if (gravity) fallSpeed -= Time.deltaTime * 10 * (fallSpeed < 0 ? 2 : 1);
            else fallSpeed = 0;

            groundObject = null;

            airDuration += Time.deltaTime;
        }

        controller.Move(new Vector3(0, fallSpeed * Time.deltaTime, 0));

        if (stepCycleDelta > 0 && moveX == 0 && moveZ == 0)
            stepCycleDelta -= Time.deltaTime * 2;
    }

    //Ekstra güçleri uygulama(Mesela: fan)
    public void AdditionalForce(Vector3 force)
    {
        controller.Move(force);
    }

    public void Blink(Vector3 newPos)
    {
        controller.enabled = false;
        controller.transform.position = newPos;
        controller.enabled = true;
    }
    
    //Zeminle entegre çalışma
    void StickToTheGround(Transform collidedObject)
    {
        if (groundObject == null)
        {
            groundObject = collidedObject;
            lastPos = groundObject.position;
        }
        else
        {
            if (collidedObject == groundObject)
            {
                controller.Move(groundObject.position - lastPos);

                lastPos = groundObject.position;
            }
            else
            {
                groundObject = collidedObject;
                lastPos = groundObject.position;
            }
        }
    }

}
