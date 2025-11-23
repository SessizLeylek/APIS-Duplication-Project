using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class MonitorSystem
{
    //Bu aktif edilebilir objeler için
    public Monitor[] monitors;
    public GameObject gameObject;

    public void ChangeStates(bool isActive)
    {
        foreach (Monitor mon in monitors)
        {
            mon.ChangeState(isActive);
        }
    }
}
