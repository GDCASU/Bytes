using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveBullet : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 2.5f;

    private float age = 0f;

    private void Update()
    {
        age += Time.deltaTime;
        if (age >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //Debug.Log("Player was struck by turret.");
        }
        Destroy(gameObject);
    }
}
