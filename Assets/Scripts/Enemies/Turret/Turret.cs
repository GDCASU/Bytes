/*
 * Author: Cristion Dominguez
 * Date: 10 Aug. 2022
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [Header("Death")]
    [SerializeField, Range(-90f, 90f)]
    private float droopAngle;
    [SerializeField]
    private float droopRotationSpeed;
    [SerializeField]
    private Transform verticalRotator;

    private TurretAttackSystem attackSystem;

    private void Awake() => attackSystem = GetComponent<TurretAttackSystem>();

    protected override void OnEnable()
    {
        base.OnEnable();
        attackSystem.Dev_OnEnable();
    }

    private void Start()
    {
        attackSystem.Dev_Start();
    }

    private void OnDisable()
    {
        attackSystem.Dev_OnDisable();
    }

    public override void ReceiveDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
            StartCoroutine(Perish());
    }

    private IEnumerator Perish()
    {
        attackSystem.Dev_OnDisable();
        float initialXAngle = verticalRotator.localEulerAngles.x;
        if (initialXAngle >= 270f && initialXAngle <= 360f) initialXAngle -= 360f;
        float finalXAngle = droopAngle;
        float dyingDuration = (droopAngle - initialXAngle) / droopRotationSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < dyingDuration)
        {
            verticalRotator.localRotation = Quaternion.Euler(Mathf.Lerp(initialXAngle, finalXAngle, elapsedTime / dyingDuration), 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        verticalRotator.localRotation = Quaternion.Euler(finalXAngle, 0f, 0f);

        enabled = false;
    }
}
