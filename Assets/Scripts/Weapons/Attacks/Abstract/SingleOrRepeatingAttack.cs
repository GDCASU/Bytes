using System.Collections;
using UnityEngine;

public abstract class SingleOrRepeatingAttack : Attack
{
    [SerializeField] protected float delayDuration;
    [SerializeField] protected bool shouldRepeat;

    protected bool isInputActive;
    protected bool isRepeatingPatternRunning;

    protected abstract float RepeatInterval { get; }

    public override void Perform(bool isStarting)
    {
        isInputActive = isStarting;

        if (isStarting && CanPerform && !isRepeatingPatternRunning)
        {
            if (shouldRepeat)
            {
                PerformManuever();
            }
            else
            {
                StartCoroutine(CR_RunRepeatingPattern());
            }
        }
    }

    public IEnumerator CR_RunRepeatingPattern()
    {
        isRepeatingPatternRunning = true;

        float elapsedTime = 0f;
        while (isInputActive && elapsedTime < delayDuration)
        {
            elapsedTime += Time.deltaTime;
            UpdateDelayProgress(elapsedTime);
            yield return null;
        }

        if (!isInputActive)
            yield break;
        else
            UpdateDelayProgress(elapsedTime);

        PerformManuever();
        elapsedTime = 0f;

        CheckIfCanPerform:
        if (CanPerform)
        {
            while (isInputActive)
            {
                if (elapsedTime < RepeatInterval)
                    elapsedTime += Time.deltaTime;
                else
                {
                    PerformManuever();
                    elapsedTime = 0f;
                    goto CheckIfCanPerform;
                }

                UpdateIntervalProgress(elapsedTime);
                yield return null;
            }
        }

        isRepeatingPatternRunning = false;
    }

    protected void StopRepeatingPattern()
    {
        StopCoroutine(CR_RunRepeatingPattern());
        isRepeatingPatternRunning = false;
    }

    protected abstract bool CanPerform { get; }
    protected abstract void UpdateDelayProgress(float elapsedTime);
    protected abstract void UpdateIntervalProgress(float elapsedTime);
    protected abstract void PerformManuever();

}