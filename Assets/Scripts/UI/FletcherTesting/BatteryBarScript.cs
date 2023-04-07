using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBarScript : MonoBehaviour
{
    
    public Slider batteryBar;
    
    // Start is called before the first frame update
    void Start()
    {
        batteryBar.maxValue = PlayerStats.playerStats.maxBattery;
    }

    // Update is called once per frame
    void Update()
    {
        batteryBar.value = PlayerStats.playerStats.battery;
        batteryBar.maxValue = PlayerStats.playerStats.maxBattery;
    }
}
