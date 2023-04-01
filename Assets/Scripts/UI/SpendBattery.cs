using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpendBattery : MonoBehaviour 
{
	public void spendPlayerBattery (int chargeCost)
	{
		PlayerStats.playerStats.spendBattery(chargeCost);
	}
}
