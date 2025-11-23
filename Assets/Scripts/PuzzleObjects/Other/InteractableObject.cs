using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [HideInInspector] public bool feedback = true; //Objenin etkileşime sonuç verip vermeyeceği; false ise etkileşimin manası olmaz
    [HideInInspector] public Outline outlineModel;

    public void Start()
    {
        outlineModel = GetComponentInChildren<Outline>();
        OnStart();
    }

    public void Update()
    {
        UpdateCycle();
    }

    public virtual void OnStart()
    {
        //Başlangıçta çalışacak
    }

    public virtual void UpdateCycle()
    {
        //Her frameda çalışacak
    }

    public virtual void OnInteract(Interaction interactedBy)
    {
        //Etkileşimde çalışacak
    }

    public virtual void OnCharge()
    {
        //Etkileşimde çalışacak
    }
}
