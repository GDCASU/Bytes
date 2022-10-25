using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public Transform Owner { get; private set; }

    private void Awake() => Owner = GetComponentInParent<Character>().transform;
}
