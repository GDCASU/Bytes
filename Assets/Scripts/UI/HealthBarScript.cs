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
        GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
        GameObject childPlayer = parentPlayer.FindGameObjectWithTag("Player");
		PlayerStats playerStats = childPlayer.GetComponentInChildren<PlayerStats>();
        healthBar.maxValue = playerStats.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerStats.getHealth();
    }
}
