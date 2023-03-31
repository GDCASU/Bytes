using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour 
{
	readonly public int limitHealth = 500; //Limits the max amount of health you can upgrade to
	readonly public int limitBattery = 10; //Limits the max amount of battery you can upgrade to
	public int maxHealth; //current limit of the full health of the player, upgradable
	public bool debugOn = false; //if true, prints values to console
	public int health;
	public int battery;
	public int maxBattery; //Sets the max battery possible, can be changed to increase charges
	
	//TODO: Add ammunition stats and more
	
	public PlayerStats() 
	{
		health = 100;
		battery = 5;
		maxBattery = 5; //Starting max battery charges, upgradable
		maxHealth = 100; 
	}

	public void takeDamage(int damage)
	{
		if ( health - damage < 0 ) 
		{
			deathEvent();
			return;
		}
		health -= damage;
		if (debugOn) 
		{
			Debug.Log("Damage Taken! " + "Health = " + health.ToString());
		}
	}

	public bool gainHealth(int heal) //Boolean could be used to show something in the UI
	{
		if (health == maxHealth) 
		{
			health = maxHealth;
			return false; //Event: Player has max health
		}
		this.health += heal;
		if (health > maxHealth) 
		{
			health = maxHealth; //Stops at maxHealth
		}
		if (debugOn) 
		{
			Debug.Log("Health Gained! " + "Health = " + health.ToString());
		}
		return true; //Event: Player healed
	}

	public bool spendBattery(int batteryCost)
	{
		if ( battery - batteryCost < 0 ) 
		{
			return false; //Dont have enough battery to spend
		}
		battery -= batteryCost;
		if (debugOn) 
		{
			Debug.Log("Battery spent! " + "Battery = " + battery.ToString());
		}
		return true; //Spent Battery
	}

	public bool gainBattery(int charges)
	{
		/* 
		According to the game design doc, battery only regenerates. This is here
		Just in case we add consumables that restore battery
		*/
		if ( battery == maxBattery ) 
		{
			return false; //Player has max battery
		}
		battery += charges;
		if ( battery > maxBattery ) 
		{
			battery = maxBattery;
		}
		if (debugOn) 
		{
			Debug.Log("Battery Gained! " + "Battery = " + battery.ToString());
		}
		return true; //Battery gained successfully
	}

	public void upgradeMaxBattery(int addedCharges) //Method upgrades the player's max amount of charges
	{
		if (maxBattery + addedCharges > limitBattery) 
		{
			batteryLimitReached();
			return;
		}
		maxBattery += addedCharges;
		if (debugOn)
		{
			Debug.Log("New Battery Limit = " + maxBattery.ToString());
		}
	}

	public bool upgradeMaxHealth(int upgradeNum) {

		if (maxHealth >= limitHealth) 
		{
			return false; //Cant upgrade health any further
		}
		if (maxHealth > limitHealth) 
		{
			maxHealth = limitHealth; //Allows last upgrade of health
		}
		if (debugOn) 
		{
			Debug.Log("New Health Limit = " + maxHealth.ToString());
		}
		return true; //Successfully upgraded Health
	}

	public void deathEvent() {
		//Place code here to execute when the player dies
	}

	public void batteryLimitReached() 
	{
		//Method to tell the player that the battery Limit cant be increased further
	}

	//Getters
	public int getHealth() 
	{
		return this.health;
	}

	public int getBattery() 
	{
		return this.battery;
	}

	//Setters
	public void setHealth(int health) 
	{
		this.health = health;
	}

	public void setBattery(int battery) 
	{
		this.battery = battery;
	}

}
