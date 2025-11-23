using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public bool isFireLit = false;
    [SerializeField] float maxFireRange = 4;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] ParticleSystem fireParticles;
    Light fireLight;
    AudioSource fireSound;
    BoxCollider col;

    void Start()
    {
        col = GetComponent<BoxCollider>();
        fireSound = GetComponent<AudioSource>();
        fireLight = fireParticles.GetComponent<Light>();

        if (isFireLit)
        {
            fireParticles.Play();
            fireSound.Play();
            fireLight.enabled = true;
        } 
        col.enabled = isFireLit;
    }

    void Update()
    {
        //Ateşin ne kadar uzağa gidebileceği raycast ile ölçülür
        if (isFireLit)
        {
            col.enabled = false;
            float fireLength = maxFireRange;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, maxFireRange))
            {
                fireLength = hit.distance;

                if (hit.transform.GetComponent<Vines>())
                    hit.transform.GetComponent<Vines>().Burn();
            }
            fireParticles.transform.localScale = new Vector3(Mathf.Max(col.size.x, 1), fireLength * 0.5f, Mathf.Max(col.size.z, 1));
            col.size = new Vector3(col.size.x, fireLength, col.size.z);
            col.center = new Vector3(0, 0.5f * fireLength, 0);

            col.enabled = true;
        }
    }

    public void ChangeFireState(bool newState)
    {
        isFireLit = newState;
        col.enabled = newState;
        GetComponent<AudioSource>().Play();

        if (newState)
        {
            fireParticles.Play();
            fireSound.Play();
            fireLight.enabled = true;
        }
        else 
        { 
            fireParticles.Stop(); 
            fireSound.Stop();
            fireLight.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<CharacterControlling>().KillPlayer();
        }
    }
}
