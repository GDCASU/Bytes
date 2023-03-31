using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpendBattery : MonoBehaviour 
{
	public void spendPlayerBattery (int chargeCost)
	{
		GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
        GameObject childPlayer = parentPlayer.FindGameObjectWithTag("Player");
		PlayerStats playerStats = childPlayer.GetComponentInChildren<PlayerStats>();
		playerStats.spendBattery(chargeCost);
	}
}
