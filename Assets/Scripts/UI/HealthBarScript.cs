using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    
    public Slider healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = PlayerStats.playerStats.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = PlayerStats.playerStats.getHealth();
    }
}
