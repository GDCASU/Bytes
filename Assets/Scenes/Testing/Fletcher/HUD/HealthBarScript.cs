using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    
    public Slider healthBar;
    PlayerStats playerStats;
    
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        healthBar.maxValue = playerStats.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerStats.getHealth();
    }
}
