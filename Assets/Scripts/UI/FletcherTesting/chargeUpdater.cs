using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chargeUpdater : MonoBehaviour
{
   //Script that changes the charge text on battery Bar
   public TMP_Text chargeText;
   private PlayerStats playerStats;
   private float calculation;

    void Awake()
    {
        this.playerStats = StatusEvents.player.getStats();
    }
    
    void Start()
    {
        chargeText.text = floorCharge().ToString();
    }

    void Update()
    {
        chargeText.text = floorCharge().ToString();
    }

    private int floorCharge()
    {
        return (int)Mathf.Floor(playerStats.battery);
    }
}
