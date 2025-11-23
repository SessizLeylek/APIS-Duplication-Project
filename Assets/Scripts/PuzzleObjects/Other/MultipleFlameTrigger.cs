using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleFlameTrigger : MonoBehaviour
{
    public FlameThrower[] flames;

    public void ChangeStatesOfFlames(bool newState)
    {
        foreach (var flame in flames)
        {
            if (flame != null)
            {
                flame.ChangeFireState(newState);
            }
        }
    }
}
