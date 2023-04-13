using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    
    public Slider healthBar;
    private PlayerStats playerStats;
    
    void Awake()
    {
        //Should be more efficient to access stats quickly
        this.playerStats = StatusEvents.player.getStats();
    }
    
    void Start()
    {
        healthBar.maxValue = this.playerStats.maxHealth;
        healthBar.value = this.playerStats.health;
    }

    void Update()
    {
        healthBar.maxValue = this.playerStats.maxHealth;
        healthBar.value = this.playerStats.health;
    }
}
