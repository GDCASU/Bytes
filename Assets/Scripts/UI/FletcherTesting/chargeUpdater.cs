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
        chargeText.text = floorCharge().ToString();
    }

    void Update()
    {
        chargeText.text = floorCharge().ToString();
    }

    private int floorCharge()
    {
        return (int)Mathf.Floor(StatusEvents.player.getBattery());
    }
}
