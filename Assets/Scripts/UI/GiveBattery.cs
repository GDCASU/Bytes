using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBattery : MonoBehaviour 
{
	public void givePlayerBattery (int chargeAmount)
	{
		GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
        GameObject childPlayer = parentPlayer.FindGameObjectWithTag("Player");
		PlayerStats playerStats = childPlayer.GetComponentInChildren<PlayerStats>();
		playerStats.gainBattery(chargeAmount);
	}
}
