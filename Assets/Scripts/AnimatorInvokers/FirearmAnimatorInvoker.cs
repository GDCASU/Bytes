using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FirearmAnimation
{
    Idle,
    Shoot,
    Reload
}

public struct FirearmAnimationData
{
    public float shootSpeed;
    public float reloadSpeed;
}

public class FirearmAnimatorInvoker : MonoBehaviour
{
    Animator animator;

    int idleStateID = Animator.StringToHash("Base Layer.Idle");
    int shootTriggerID = Animator.StringToHash("Shoot");
    int mustReloadID = Animator.StringToHash("MustReload");
    int shootSpeedID = Animator.StringToHash("ShootSpeed");
    int reloadSpeedID = Animator.StringToHash("ReloadSpeed");

    public void Bind(Animator animator)
    {
        animator.keepAnimatorControllerStateOnDisable = true;
        this.animator = animator;
    }

    public void SetParameters(FirearmAnimationData data)
    {
        animator.SetFloat(shootSpeedID, data.shootSpeed);
        animator.SetFloat(reloadSpeedID, data.reloadSpeed);
    }

    public void ResetAnimator() => animator.Play(idleStateID);

    public void Play(FirearmAnimation animation)
    {
        switch (animation)
        {
            case FirearmAnimation.Idle:
                animator.Play(idleStateID);
                break;
            case FirearmAnimation.Shoot:
                animator.SetTrigger(shootTriggerID);
                break;
            case FirearmAnimation.Reload:
                animator.SetBool(mustReloadID, true);
                break;
        }
    }
}