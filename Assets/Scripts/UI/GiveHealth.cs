using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveHealth : MonoBehaviour 
{
	public void healPlayer (int healAmount)
	{
		GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
        GameObject childPlayer = parentPlayer.FindGameObjectWithTag("Player");
		PlayerStats playerStats = childPlayer.GetComponentInChildren<PlayerStats>();
		playerStats.gainHealth(healAmount);
	}
}
