using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponTester : MonoBehaviour
{
    Weapon2 weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon2>();
    }
}
