using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryBarScript : MonoBehaviour
{
    
    public Slider batteryBar;
    PlayerStats playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        batteryBar.maxValue = playerStats.maxBattery;
    }

    // Update is called once per frame
    void Update()
    {
        batteryBar.value = playerStats.battery;
        batteryBar.maxValue = playerStats.maxBattery;
    }
}
