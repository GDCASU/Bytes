/*
 * Author: Cristion Dominguez
 * Date: 19 Jan. 2023
 */


using UnityEngine;

public class Dummy : MonoBehaviour
{
    void Awake() => GetComponent<Damageable>().DamageReceived += PrintDamage;

    void PrintDamage(int damage) => Debug.Log("Dummy has received " + damage + " damage.");
}
