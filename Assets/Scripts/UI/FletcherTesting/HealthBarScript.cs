using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBar;
    
    void Start()
    {
        healthBar.maxValue = StatusEvents.player.getMaxHealth();
        healthBar.value = StatusEvents.player.getHealth();
    }

    void Update()
    {
        healthBar.maxValue = StatusEvents.player.getMaxHealth();
        healthBar.value = StatusEvents.player.getHealth();
    }
}
