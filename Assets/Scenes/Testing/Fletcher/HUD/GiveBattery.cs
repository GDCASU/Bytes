﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveBattery : MonoBehaviour 
{
	public void givePlayerBattery (int chargeAmount)
	{
		PlayerStats targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
		targetPlayer.gainBattery(chargeAmount);
	}
}
