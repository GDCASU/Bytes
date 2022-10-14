using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] InputReader _inputReader = default;

    private void Awake() => _inputReader.EnableGameplayInput();
}
