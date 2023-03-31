using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour 
{
	public bool debugOn = false; //if true, prints values to console
	readonly public int limitHealth = 500; //Limits the max amount of health you can upgrade to
	readonly public int limitBattery = 10; //Limits the max amount of battery you can upgrade to
	public int maxBattery; //Sets the max battery possible, upgradeable
	public int maxHealth; //current limit of the full health of the player, upgradeable
	public int health; //Current health of the player
	public int battery; //Current battery of the player
	
	//TODO: Add ammunition inventory and more
	
	public PlayerStats() 
	{
		maxHealth = 100;
		maxBattery = 5;
		health = 100;
		battery = 5;
	}

	public void takeDamage(int damage)
	{
		int newHealth = health - damage;
		if ( newHealth > 0 ) //Enough Health to tank incoming damage
		{
			health = newHealth;
			if (debugOn) 
			{
				Debug.Log("Damage Taken! " + "Health = " + health.ToString());
			}
		}
		else 
		{
			deathEvent(); //Died, not implemented yet
		}
	}

	public bool gainHealth(int heal) //Boolean could be used to show something in the UI
	{
		if (health < maxHealth) 
		{
			health += heal;
			if (health > maxHealth) 
			{
				health = maxHealth; //If health went over, set it back to max
			}
			if (debugOn) 
			{
				Debug.Log("Health Gained! " + "Health = " + health.ToString());
			}
			return true; //Event: Player healed
		}
		return false; //Event: Player has max health
	}

	public bool spendBattery(int batteryCost)
	{
		int newBattery = battery - batteryCost;
		if ( newBattery >= 0 ) 
		{
			battery = newBattery;
			if (debugOn) 
			{
				Debug.Log("Battery spent! " + "Battery = " + battery.ToString());
			}
			return true; //Spent battery
		}
		return false; //Don't have enough battery
	}

	public bool gainBattery(int charges)
	{
		/* 
		According to the game design doc, battery only regenerates. This is here
		Just in case we add consumables that restore battery
		*/
		if (battery < maxBattery) 
		{
			battery += charges;
			if (battery > maxBattery) 
			{
				battery = maxBattery; 
			}
			if (debugOn) 
			{
				Debug.Log("Battery Gained! " + "Battery = " + battery.ToString());
			}
			return true; //Battery gained successfully
		}
		return false; //Player has max battery
	}

	public bool upgradeMaxBattery(int addedCharges) //Method upgrades the player's max amount of charges
	{
		if (maxBattery < limitBattery) 
		{
			maxBattery += addedCharges;
			if (maxBattery > limitBattery)
			{
				maxBattery = limitBattery;
			}
			if (debugOn)
			{
				Debug.Log("New Battery Limit = " + maxBattery.ToString());
			}
			return true; //Battery upgraded successfully
		}
		return false; //Cant upgrade battery anymore
	}

	public bool upgradeMaxHealth(int upgradeNum) 
	{
		if (maxHealth < limitHealth) 
		{
			maxHealth += upgradeNum;
			if (maxHealth > limitHealth)
			{
				maxHealth = limitHealth;
			}
			if (debugOn) 
			{
				Debug.Log("New Health Limit = " + maxHealth.ToString());
			}
			return true; //Successfully upgraded Health
		}
		return false; //Cant upgrade health any further
	}

	public void deathEvent() {
		//Place code here to execute when the player dies
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
