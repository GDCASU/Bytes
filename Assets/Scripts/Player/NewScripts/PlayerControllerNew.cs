using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControllerNew : MonoBehaviour
{
    PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }
}
