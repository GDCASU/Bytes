using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBarScript : MonoBehaviour
{
    public Slider batteryBar;
    
    void Start()
    {
        batteryBar.maxValue = StatusEvents.player.getMaxBattery();
        batteryBar.value = StatusEvents.player.getBattery();
    }

    void Update()
    {
        batteryBar.maxValue = StatusEvents.player.getMaxBattery();
        batteryBar.value = StatusEvents.player.getBattery();
    }
}
