using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using static UnityEngine.ParticleSystem;

public class WaterRippleEffect : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float fixedYPosition;
    [SerializeField] Vector3 waterArea1;
    [SerializeField] Vector3 waterArea2;
    ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        EmissionModule psEmission = particleSystem.emission;
        if ((waterArea1.x - playerTransform.position.x) * (waterArea2.x - playerTransform.position.x) < 0 && (waterArea1.y - playerTransform.position.y) * (waterArea2.y - playerTransform.position.y) < 0 && (waterArea1.z - playerTransform.position.z) * (waterArea2.z - playerTransform.position.z) < 0)
        {
            if (Vector3.Distance(playerTransform.position - Vector3.up * playerTransform.position.y, transform.position - Vector3.up * transform.position.y) < 0.2f)
                particleSystem.enableEmission = true;
                //psEmission.rateOverDistanceMultiplier = 1;

            Vector3 newPos = playerTransform.position;
            newPos.y = fixedYPosition;
            transform.position = newPos;

        }
        else
        {
            particleSystem.enableEmission = false;
            //psEmission.rateOverDistanceMultiplier = 0;
        }
    }
}
