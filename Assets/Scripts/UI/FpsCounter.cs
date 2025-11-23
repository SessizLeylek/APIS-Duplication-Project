using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    Text text;
    List<float> fpsValues = new List<float>();
    int visible = 0;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            visible = 1 - visible;
            transform.localScale = Vector3.one * visible;
        }
        fpsValues.Add(1 / Time.unscaledDeltaTime);
        while(fpsValues.Count > 1 / Time.unscaledDeltaTime) fpsValues.RemoveAt(0);

        text.text = String.Format("{0:0.00}", fpsValues.Average()) + " FPS";
    }
}
