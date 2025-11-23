using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaTetraleg : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] Transform phase1Puzzles;
    [SerializeField] GameObject caveParticles;

    Animator animator;
    TetralegBodyParts[] bodyParts;
    float chaseStartTime = 0f;
    bool mechaWalking = false;
    float speed = 6;
    int health = 4;
    float prevMechaBurnPos = 120f;

    void Start()
    {
        animator = GetComponent<Animator>();
        bodyParts = GetComponentsInChildren<TetralegBodyParts>();
    }
    
    void Update()
    {
        if (mechaWalking)
        {
            //Could need a rework
            float remainingTime = 62 - Time.time + chaseStartTime - health * 8;
            
            if(remainingTime * speed < transform.position.x + 32)
                speed += Time.deltaTime * 1.6f;

            animator.SetFloat("speed", speed);
            transform.position += Vector3.back * Time.deltaTime * speed;
        }
    }

    public void StartChasing()
    {
        mechaWalking = true;
        animator.Play("MechaWalking");

        chaseStartTime = Time.time;

        GameObject explEff = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explEff, 5);

        FindObjectOfType<CharacterControlling>().camera.ShakeCamera(1, 15);
        caveParticles.SetActive(true);
        phase1Puzzles.gameObject.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (mechaWalking && other.CompareTag("Player"))
        {
            mechaWalking = false;
            animator.Play("MechaJumpscare");
            FindObjectOfType<SfxPlayer>().PlaySFX(13, transform.position + new Vector3(0, 3, -12));

            StartCoroutine(MechaJumpscaring(other.GetComponent<CharacterControlling>()));
        }

        if (other.CompareTag("Fire"))
        {
            if (transform.position.z < prevMechaBurnPos)
            {
                health--;
                prevMechaBurnPos = transform.position.z - 10f;
                StartCoroutine(MechaBurning());
            }
        }
    }

    IEnumerator MechaJumpscaring(CharacterControlling character)
    {
        character.movement.canMove = false;
        character.camera.SetCameraFreedom(true);
        
        Vector3 camInitialPos = character.camera.camera.transform.position;
        Quaternion camInitialRot = character.camera.camera.transform.rotation;
        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < 0.25f)
        {
            character.camera.camera.transform.position = Vector3.Lerp(camInitialPos, transform.position + new Vector3(0, 3, -12), (Time.time - lerpStartTime) * 4);
            character.camera.camera.transform.rotation = Quaternion.Lerp(camInitialRot, Quaternion.identity, (Time.time - lerpStartTime) * 4);
            yield return null;
        }

        yield return new WaitForSeconds(0.6f);

        character.KillPlayer();
    }

    IEnumerator MechaBurning()
    {
        mechaWalking = false;
        GetComponent<Collider>().isTrigger = false;

        animator.SetBool("burning", true);
        yield return new WaitForSeconds(6f);

        GameObject explEff = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explEff, 5);

        GetComponent<Collider>().isTrigger = true;

        FindObjectOfType<CharacterControlling>().camera.ShakeCamera(2, 15);

        if (health < 1)
            StartCoroutine(LastExplosion());
        else
        {
            speed = 0;
            animator.SetBool("burning", false);
            mechaWalking = true;

            foreach(TetralegBodyParts part in bodyParts)
            {
                part.ChangeBodyMesh(health - 1);
            }
        }
    }

    IEnumerator LastExplosion()
    {
        transform.localScale = Vector3.zero;

        CharacterControlling character = FindObjectOfType<CharacterControlling>();
        character.movement.canMove = false;
        character.movement.gravity = false;

        float lerpStartTime = Time.time;
        while (Time.time - lerpStartTime < 6)
        {
            float dTime = Time.time - lerpStartTime;
            character.movement.Blink(new Vector3(28.5f, 28 - (dTime - 3) * (dTime - 3) * 2.94f, dTime * -22.5f - 45));
            yield return null;
        }
        while (Time.time - lerpStartTime < 6.5f)
        {
            float dTime = Time.time - lerpStartTime;
            character.movement.Blink(new Vector3(28.5f, 1.54f, dTime * -10f - 120));
            yield return null;
        }

        character.movement.canMove = true;
        character.movement.gravity = true;
        Destroy(gameObject);
    }

}
