using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    [SerializeField] Material monitorWhite;
    [SerializeField] Material monitorYellow;
    MeshRenderer meshRenderer;
    
    public void ChangeState(bool isActivated)
    {
        if(meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        Material[] oldMaterials = meshRenderer.materials;

        //Change Monitor Color
        if (isActivated)
            oldMaterials[2] = monitorYellow;
        else
            oldMaterials[2] = monitorWhite;

        meshRenderer.materials = oldMaterials;
    }
}
