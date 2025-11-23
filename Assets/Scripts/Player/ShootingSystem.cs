using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

[System.Serializable]
public class ShootingSystem
{
    public Transform pistolModel;
    public GameObject chargedPistolEffect;
    public LineRenderer laserBeam;

    [HideInInspector] public BurstParticleCreator particleCreator;
    Transform shootOrigin;
    List<Vector3> hitPoints = new List<Vector3>();
    [HideInInspector] public bool isCharged = false;

    Vector3 pistolTipPos;
    public void PistolMovementUpdate(Vector3 playerForward)
    {
        pistolModel.forward = pistolTipPos - pistolModel.position;
        pistolModel.RotateAround(pistolModel.position, pistolModel.forward, -90);
        pistolModel.RotateAround(pistolModel.position, pistolModel.up, -90);

        pistolTipPos = pistolTipPos * (1 - Time.deltaTime * 10) + pistolModel.position * Time.deltaTime * 10 + playerForward;
    }

    public void SetChargeState(bool newState)
    {
        isCharged = newState;

        //Efekt
        chargedPistolEffect.SetActive(isCharged);
    }

    public void Shoot(Transform transform)
    {
        if (isCharged && Time.timeScale == 1)
        {
            //Ateşleme
            SetChargeState(false);

            GameObject.FindObjectOfType<SfxPlayer>().PlaySFX(6, transform.position);
            if (shootOrigin == null) shootOrigin = pistolModel.GetChild(0);

            hitPoints.Add(shootOrigin.position);

            Vector3 beamPosition = transform.position;
            Vector3 beamDirection = transform.forward;

            RaycastHit hit;
            bool continueWhile = true;
            float infiniteLoopBlocker = 0;
            while(continueWhile)
            {
                if (Physics.Raycast(beamPosition + beamDirection * 0.35f, beamDirection, out hit, 25))
                {
                    hitPoints.Add(hit.point);

                    if (hit.transform.tag == "ReflectionSurface")
                    {
                        beamPosition = hit.point;
                        Vector3 reflectedVector = Vector3.Reflect(beamDirection, hit.transform.forward);

                        Lever aimedLever = null;
                        float aimedLeverPrecision = 0f;
                        Lever[] levers = GameObject.FindObjectsOfType<Lever>();
                        foreach (Lever lever in levers)
                        {
                            if(Vector3.Distance(lever.transform.position, hit.point) < 25)
                            {
                                Vector3 leverAimVector = (lever.transform.position - hit.point).normalized;
                                float precision = Vector3.Dot(leverAimVector, reflectedVector.normalized);
                                if (precision > 0.96f)
                                {
                                    if (aimedLever)
                                    {
                                        if (aimedLeverPrecision < precision)
                                        {
                                            aimedLever = lever;
                                            aimedLeverPrecision = precision;
                                        }
                                    }
                                    else
                                    {
                                        aimedLever = lever;
                                        aimedLeverPrecision = precision;
                                    }
                                }
                            }
                        }

                        if (aimedLever)
                        {
                            beamDirection = (aimedLever.transform.position - hit.point).normalized;
                        }
                        else
                        {
                            beamDirection = reflectedVector;
                        }
                    }
                    else
                    {
                        continueWhile = false;
                        if (hit.transform.tag == "Interactable")
                        {
                            hit.transform.GetComponent<InteractableObject>().OnCharge();
                        }
                    }
                }
                else
                {
                    continueWhile = false;
                    hitPoints.Add(beamPosition + (beamDirection * 25));
                }

                infiniteLoopBlocker++;
                if(infiniteLoopBlocker > 20) continueWhile= false;
            }

            //Efekt oluşturma
            particleCreator.CreateEffect(1, hitPoints[hitPoints.Count - 1], new Vector3(-90, 0, 0));
        }
    }

    //Silah tepmesi animasyonu
    public IEnumerator PistolRecoil()
    {
        if (shootOrigin == null) shootOrigin = pistolModel.GetChild(0);

        laserBeam.positionCount = hitPoints.Count;
        laserBeam.SetPositions(hitPoints.ToArray());
        laserBeam.widthMultiplier = 0.1f;

        for (int rot = 270; rot > 250; rot -= 4)
        {
            pistolModel.localEulerAngles = new Vector3(rot, 0, 270);
            if(hitPoints.Count > 0) laserBeam.SetPosition(0, shootOrigin.position);
            yield return null;
        }

        for (int rot = 250; rot < 270; rot++)
        {
            pistolModel.localEulerAngles = new Vector3(rot, 0, 270);
            laserBeam.widthMultiplier -= 0.005f;
            yield return null;
        }
        laserBeam.widthMultiplier = 0;

        hitPoints.Clear();
    }
}
