using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveHealth : MonoBehaviour 
{
	public void healPlayer (int healAmount)
	{
		PlayerStats targetPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
		targetPlayer.gainHealth(healAmount);
	}
}
