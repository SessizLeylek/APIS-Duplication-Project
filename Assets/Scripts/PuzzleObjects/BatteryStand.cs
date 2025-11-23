using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatteryStand : InteractableObject
{
    public bool isPowered = false;
    public bool secondaryState = false;
    public UnityEvent whenBatteryPut;
    public UnityEvent whenBatteryRemove;
    public MonitorSystem monitorSystem;
    [SerializeField] PuzzleCable[] cables;

    GameObject batteryObject;

    public override void OnStart()
    {
        batteryObject = transform.GetChild(0).gameObject;

        int lastSaveId = PlayerPrefs.GetInt("progress");
        if (lastSaveId / 2 == UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2 && lastSaveId % 2 == 1)
        {
            isPowered = !secondaryState;
            if (isPowered) RemoveBattery();
            else PutBattery();
        }

        feedback = isPowered;
        batteryObject.SetActive(isPowered);
        monitorSystem.ChangeStates(isPowered);
        foreach (PuzzleCable cable in cables)
        {
            cable.ChangeCablePowered(isPowered);
        }
    }

    public override void OnInteract(Interaction interactedBy)
    {
        if (isPowered && !interactedBy.haveBattery)
        {
            RemoveBattery();
            interactedBy.haveBattery = true;
            interactedBy.holdingItem = 1;
            interactedBy.holdItem.ChangeItem("battery", transform.position);

            feedback = true;
        }
        else if (!isPowered && interactedBy.haveBattery && interactedBy.holdingItem == 1)
        {
            PutBattery();
            interactedBy.haveBattery = false;
            if (interactedBy.havePistol)
            {
                interactedBy.holdingItem = 2;
                interactedBy.holdItem.ChangeItem("pistol", interactedBy.transform.position);
            }
            else
            {
                interactedBy.holdingItem = 0;
                interactedBy.holdItem.ChangeItem("", Vector3.zero);
            }

            feedback = true;
        }
        else
        {
            feedback = false;
        }
    }

    public void PutBattery()
    {
        if (!isPowered)
        {
            isPowered = true;

            batteryObject.SetActive(true);

            whenBatteryPut.Invoke();
            
            monitorSystem.ChangeStates(true);
            foreach(PuzzleCable cable in cables)
            {
                cable.ChangeCablePowered(true);
            }
        }
    }

    public void RemoveBattery()
    {
        if (isPowered)
        {
            isPowered = false;

            batteryObject.SetActive(false);

            whenBatteryRemove.Invoke();

            monitorSystem.ChangeStates(false);
            foreach (PuzzleCable cable in cables)
            {
                cable.ChangeCablePowered(false);
            }
        }
    }

}
