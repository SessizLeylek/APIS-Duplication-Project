using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricOutlet : InteractableObject
{
    public bool isPowered;
    [SerializeField] GameObject chargeEffect;

    GameObject electricEffect;

    public override void OnStart()
    {
        electricEffect = transform.GetChild(0).gameObject;

        electricEffect.SetActive(isPowered);
    }

    public override void OnInteract(Interaction interactedBy)
    {
        if (interactedBy.holdingItem == 2 && isPowered)
        {
            interactedBy.shootSystem.SetChargeState(true);
            GameObject.FindObjectOfType<SfxPlayer>().PlaySFX(5, transform.position);
            GameObject createdEffect = Instantiate(chargeEffect, transform.position, Quaternion.LookRotation((interactedBy.transform.GetComponentInChildren<Light>().transform.position/*This is awful*/ - transform.position).normalized, Vector3.up));
            Destroy(createdEffect, 1);

            feedback = true;
        }else
        {
            feedback = false;
        }
    }

    public void ChangePower(bool power)
    {
        isPowered = power;
        electricEffect.SetActive(isPowered);
    }
}
