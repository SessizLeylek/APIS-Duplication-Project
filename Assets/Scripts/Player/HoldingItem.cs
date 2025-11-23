using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HoldingItem
{
    //Tutulan eşyanın nesneler içine girmemesi için
    public Transform originPoint;
    public GameObject battery;
    public GameObject pistol;
    [SerializeField] GameObject characterModelBattery;
    [SerializeField] GameObject characterModelPistol;

    Transform item;
    string itemName = "";

    public void ItemPositionUpdate()
    {
        if(item) item.position = Vector3.Lerp(item.position, originPoint.position, Time.deltaTime * 6);
    }

    public void ChangeItem(string name, Vector3 initialPos)
    {
        itemName = name;
        
        battery.SetActive(false);
        characterModelBattery.SetActive(false);
        pistol.SetActive(false);
        characterModelPistol.SetActive(false);
        item = null;

        if (name == "battery")
        {
            battery.SetActive(true);
            characterModelBattery.SetActive(true);
            item = battery.transform;
            item.position = initialPos;
        }
        else if (name == "pistol")
        {
            pistol.SetActive(true);
            characterModelPistol.SetActive(true);
            item = pistol.transform;
            item.position = initialPos;
        }
    }
}
