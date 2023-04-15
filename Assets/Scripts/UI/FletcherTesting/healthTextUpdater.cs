using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class healthTextUpdater : MonoBehaviour
{
   //Script that changes the charge text on battery Bar
   public TMP_Text healthText;
    
    void Start()
    {
        healthText.text = healthFloor().ToString();
    }

    void Update()
    {
        healthText.text = healthFloor().ToString();
    }

    private float healthFloor()
    {
        //Also cuts the float to one decimal place
        float health = StatusEvents.player.getHealth();
        return Mathf.Floor(health * 10.0f) * 0.1f;
    }
}
