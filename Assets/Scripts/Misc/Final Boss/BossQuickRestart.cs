using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossQuickRestart : MonoBehaviour
{
    // Tüm asansörler indirilmişse, yeniden doğmak otomatik olarak onları geri indirir
    [SerializeField] Elevator[] elevators;
    [SerializeField] BatteryStand outletBattery;
    [SerializeField] CharacterControlling character;
    [SerializeField] GameObject musicTrigger;
    static bool quickRestart = false;

    void Start()
    {
        if(quickRestart)
        {
            elevators[0].MovePlatform(false);
            elevators[1].MovePlatform(false);
            elevators[2].MovePlatform(false);
            outletBattery.PutBattery();
            character.interaction.haveBattery = false;
            character.interaction.ChangeItem();
            character.interaction.shootSystem.SetChargeState(true);
            character.movement.Blink(new Vector3(28.5f, 1.5f, 105f));

            musicTrigger.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightShift) && Input.GetKey(KeyCode.Alpha9))
            quickRestart = true;
    }

    public void CheckElevators()
    {
        if (!elevators[0].isOnTop && !elevators[1].isOnTop && !elevators[2].isOnTop)
            quickRestart = true;
    }
}
