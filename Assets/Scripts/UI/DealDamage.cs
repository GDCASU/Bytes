using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour 
{
	public void sendDamage (int dmg)
	{
		PlayerStats.playerStats.takeDamage(dmg);
	}
}
