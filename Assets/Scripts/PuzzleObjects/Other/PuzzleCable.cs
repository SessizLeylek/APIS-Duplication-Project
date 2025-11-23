using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCable : MonoBehaviour
{
    //Bağlı olduğu aktif edilince parlar
    [SerializeField] Material unpoweredMat;
    [SerializeField] Material poweredMat;
    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer= GetComponent<MeshRenderer>();
    }

    public void ChangeCablePowered(bool newState)
    {
        meshRenderer.material = newState ? poweredMat : unpoweredMat;
    }
}
