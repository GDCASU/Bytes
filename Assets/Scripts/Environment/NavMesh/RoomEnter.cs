using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnter : MonoBehaviour
{
    [SerializeField] NavMeshEnemy[] navMeshEnemies;

    private void OnTriggerEnter(Collider other)
    {
        foreach (NavMeshEnemy enemy in navMeshEnemies) enemy.SetTarget(other.gameObject);
    }
}
