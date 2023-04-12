using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBarScript : MonoBehaviour
{
    public Slider batteryBar;
    private PlayerStats playerStats;
    
    void Awake()
    {
        //Should be more efficient to access stats quickly
        this.playerStats = StatusEvents.statusEvents.getStats();
    }
    
    void Start()
    {
        batteryBar.maxValue = playerStats.maxBattery;
        batteryBar.value = playerStats.battery;
    }

    void Update()
    {
        batteryBar.maxValue = playerStats.maxBattery;
        batteryBar.value = playerStats.battery;
    }
}
