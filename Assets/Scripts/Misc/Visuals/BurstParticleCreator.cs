using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstParticleCreator : MonoBehaviour
{
    public ParticleSystem[] particles;

    public void CreateEffect(int effectID, Vector3 loc, Vector3 euler)
    {
        GameObject obj = Instantiate(particles[effectID].gameObject, loc, Quaternion.Euler(euler));

        Destroy(obj, particles[effectID].startLifetime);
    }
}
