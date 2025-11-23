using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticle : MonoBehaviour
{
    ParticleSystem sys;
    float deltaTime;

    void Start()
    {
        sys = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        deltaTime += Time.deltaTime;
        if (deltaTime > sys.duration)
            Destroy(gameObject);
    }
}
