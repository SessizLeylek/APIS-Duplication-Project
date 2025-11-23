using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OtherInteractable : InteractableObject
{
    public UnityEvent onInteract;

    public override void OnInteract(Interaction interactedBy)
    {
        onInteract.Invoke();
    }
}
