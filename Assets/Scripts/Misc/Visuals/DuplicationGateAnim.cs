using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicationGateAnim : MonoBehaviour
{
    public Material mat;

    void Start()
    {
        
    }

    void Update()
    {
        float texOffset = mat.mainTextureOffset.x;
        if (texOffset < 0)
        {
            mat.mainTextureOffset = new Vector2(texOffset + 1, texOffset + 1);
        }else
        {
            mat.mainTextureOffset = new Vector2(texOffset - (Time.deltaTime / 5f), texOffset - (Time.deltaTime / 5f));
        }

    }
}
