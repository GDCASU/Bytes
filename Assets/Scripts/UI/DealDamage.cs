using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour 
{
	public void sendDamage (int dmg)
	{
		GameObject parentPlayer = GameObject.FindGameObjectWithTag("Player");
        GameObject childPlayer = parentPlayer.FindGameObjectWithTag("Player");
		PlayerStats playerStats = childPlayer.GetComponentInChildren<PlayerStats>();
		playerStats.takeDamage(dmg);
	}
}
