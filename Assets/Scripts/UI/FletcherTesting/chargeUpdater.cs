using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chargeUpdater : MonoBehaviour
{
   //Script that changes the charge text on battery Bar
   public TMP_Text chargeText;
    
    void Start()
    {
        chargeText.text = (PlayerStats.playerStats.getBattery()).ToString();
    }

    void Update()
    {
        chargeText.text = (PlayerStats.playerStats.getBattery()).ToString();
    }
}
