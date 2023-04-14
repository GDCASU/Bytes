using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class handles all outside interactions that need to alter
 * anything contained on PlayerStats and the UI.
 * Use StatusEvents.player.method to access anything here from within
 * other scripts around the game.
*/


public class StatusEvents : MonoBehaviour
{
    public static StatusEvents player;
    public GameObject playerStatsHolder; //Assign to object that holds playerStats
    public GameObject UIManagerHolder; //Assign to object that holds the UI Manager
    private CrosshairSpread crosshairSpread;
    private PlayerStats playerStats;
    private HealthBarScript healthBar;

    //Testing
    public bool gain2Battery = false; //Bool to test Battery gain
    public bool heal20HP = false; //Bool to test out Health gain
    public bool spend3Battery = false; //Bool to test out battery regen without buttons
    public bool deal30Damage = false; //Bool to test out Health acceleration
    public bool dealPoint5Damage = false; //Bool to test HP float display

    private void Awake()
    {
        playerStats = playerStatsHolder.GetComponent<PlayerStats>();
        crosshairSpread = UIManagerHolder.GetComponent<CrosshairSpread>();
        healthBar = UIManagerHolder.GetComponent<HealthBarScript>();
        player = this;
    }

    private void Update()
    {
        //Testing bools
        if (spend3Battery)
        {
            useBattery(3f);
            spend3Battery = false;
        }
        if (deal30Damage)
        {
            sendDamage(30);
            deal30Damage = false;
        }
        if (heal20HP) 
        {
            sendHealth(20);
            heal20HP = false;
        }
        if (gain2Battery)
        {
            sendBattery(2f);
            gain2Battery = false;
        }
        if (dealPoint5Damage)
        {
            sendDamage(0.5f);
            dealPoint5Damage = false;
        }
        this.playerStats.regenBattery();
    }

    /*
    TODO: ADD BATTERY RECHARGING
    */
    
    private void deathEvent()
    {
        //TODO: Will trigger with health check
    }
    
    public void setSpread(float spread)
    {
        crosshairSpread.spreadChange(spread);
    }

    public void sendDamage(float damage)
    {
        bool result = playerStats.recieveDamage(damage);
        healthBar.AlertDamage();
        if (!result) {
            deathEvent(); //Calls death event when health was not enough to tank damage
        }
    }

    public void sendHealth(float heal)
    {
        bool result = playerStats.recieveHealth(heal);
        if (result)
        {
            //Player Healed! Leaving this here in case its useful for interacting with something else
        }
        else
        {
            //Player at Max Health
        }
    }

    public void useBattery(float cost)
    {
        bool result = playerStats.spendBattery(cost);
        if (result)
        {
            //Battery was enough to take the cost
        }
        else
        {
            //Battery charge wasnt enough for cost
        }
    }

    public void sendBattery(float charge)
    {
        bool result = playerStats.recieveBattery(charge);
        if (result)
        {
            //Player gained battery
        }
        else
        {
            //Player at max battery
        }
    }

    public void upgradeMaxBattery(float batUpgrade)
    {
        bool result = playerStats.raiseMaxBattery(batUpgrade);
        if (result)
        {
            //Battery Max Upgraded
        }
        else
        {
            //Battery Upgrade Limit reached
        }
    }

    public void upgradeMaxHealth(float hpUpgrade)
    {
        bool result = playerStats.raiseMaxHealth(hpUpgrade);
        if (result)
        {
            //Max Health Upgraded
        }
        else
        {
            //Health Upgrade Limit reached
        }
    }
    
    //Methods neccesary for status bars to work
    public float getHealth()
    {
        return playerStats.health;
    }

    public float getBattery()
    {
        return playerStats.battery;
    }

    public float getMaxHealth()
    {
        return playerStats.maxHealth;
    }

    public float getMaxBattery()
    {
        return playerStats.maxBattery;
    }

}
