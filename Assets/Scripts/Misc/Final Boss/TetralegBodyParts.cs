using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetralegBodyParts : MonoBehaviour
{
    //Parçalanma efektini verebilmek için parçaları değişir
    [SerializeField] Mesh[] bodyMeshes;

    public void ChangeBodyMesh(int newMeshIndex)
    {
        GetComponent<MeshFilter>().mesh = bodyMeshes[newMeshIndex];
    }
}
