using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpendBattery : MonoBehaviour 
{
	public void spendPlayerBattery (int chargeCost)
	{
		PlayerStats targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
		targetPlayer.spendBattery(chargeCost);
	}
}
