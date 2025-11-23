using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTrigger : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
            onEnter.Invoke();
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
            onExit.Invoke();
    }
}
