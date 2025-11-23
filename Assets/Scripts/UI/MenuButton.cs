using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    Text text;
    bool onMouse = false;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if(onMouse && text.color.b > 0)
        {
            text.color = new Color(1, 1, text.color.b - 0.06f);
        }
        else if (!onMouse && text.color.b < 1)
        {
            text.color = new Color(1, 1, text.color.b + 0.04f);
        }
    }

    public void MouseEnter()
    {
        onMouse = true;
        text.fontStyle = FontStyle.Bold;
    }

    public void MouseExit()
    {
        onMouse = false;
        text.fontStyle = FontStyle.Normal;
    }
}
