using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IEnemy
{
    [Header("Basic Properties")]
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private Transform[] detectionPoints;

    [Header("Death")]
    [SerializeField, Range(-90f, 90f)]
    private float droopAngle;
    [SerializeField]
    private float droopRotationSpeed;
    [SerializeField]
    private Transform verticalRotator;

    [Header("Read-Only Properties")]
    [SerializeField]
    private float health;

    private TurretAttackSystem attackSystem;

    private void Awake()
    {
        health = maxHealth;
        attackSystem = GetComponent<TurretAttackSystem>();
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            StartCoroutine(Perish());
    }

    public void ReceiveHealth(float addedHealth)
    {
        health += addedHealth;
        if (health > maxHealth)
            health = maxHealth;
    }

    private IEnumerator Perish()
    {
        attackSystem.enabled = false;
        Quaternion startRotation = verticalRotator.localRotation;
        Quaternion endRotation = Quaternion.Euler(droopAngle, 0f, 0f);
        float dyingDuration = (droopAngle - verticalRotator.localEulerAngles.x) / droopRotationSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < dyingDuration)
        {
            verticalRotator.localRotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / dyingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        verticalRotator.localRotation = endRotation;

        enabled = false;
    }

    public Transform[] GetDetectionPoints() => detectionPoints;
}
