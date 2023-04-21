using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public float rotationSpeedX = 0.0f;
    public float rotationSpeedY = 0.7f;
    public float rotationSpeedZ = 0.0f;

    // Variables for the bobbing effect
    public float bobbingSpeed = 1f;
    public float bobbingHeight = 0.1f;
    private float startY;

    void Start()
    {
        startY = transform.position.y + 0.2f;
    }

    void Update()
    {
        SpinObject();   
    }

    void SpinObject()
    {
        transform.Rotate(rotationSpeedX, rotationSpeedY, rotationSpeedZ);

        float newY = startY + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
