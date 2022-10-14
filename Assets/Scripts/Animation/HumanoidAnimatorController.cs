using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int velocityXHash = Animator.StringToHash("VelocityX");
    int velocityZHash = Animator.StringToHash("VelocityZ");

    float velocityX;
    float velocityZ;
    [SerializeField] float acceleration = 0.2f;
    [SerializeField] float decceleration = 0.5f;
    float maxWalkVelocity = 0.5f;
    float maxRunVelocity = 2f;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        bool forwardPressed = Keyboard.current.upArrowKey.isPressed;
        bool leftPressed = Keyboard.current.leftArrowKey.isPressed;
        bool rightPressed = Keyboard.current.rightArrowKey.isPressed;
        bool runPressed = Keyboard.current.shiftKey.isPressed;
        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        ChangeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
        ClampVelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);

        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityZHash, velocityZ);
    }

    void ChangeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (leftPressed && rightPressed)
        {
            if (velocityX < 0f)
            {
                velocityX += Time.deltaTime * decceleration;
            }
            else if (velocityX > 0f)
            {
                velocityX -= Time.deltaTime * decceleration;
            }
        }
        else if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        else if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocityZ > 0f)
        {
            velocityZ -= Time.deltaTime * decceleration;
        }

        if (!forwardPressed && velocityZ < 0f)
        {
            velocityZ = 0f;
        }

        if (!leftPressed && velocityX < 0f)
        {
            velocityX += Time.deltaTime * decceleration;
        }

        if (!rightPressed && velocityX > 0f)
        {
            velocityX -= Time.deltaTime * decceleration;
        }
    }

    void ClampVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        // Reset Forward
        if (!forwardPressed && velocityZ < 0f)
        {
            velocityZ = 0f;
        }

        // Reset Left and Right
        if (!leftPressed && !rightPressed && velocityX != 0f && (velocityX > -0.01f && velocityX < 0.01f))
        {
            velocityX = 0f;
        }

        // Lock Forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * decceleration;

            if (velocityZ > currentMaxVelocity && velocityZ < currentMaxVelocity + 0.01f)
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > currentMaxVelocity - 0.01f)
        {
            velocityZ = currentMaxVelocity;
        }

        // Lock Left
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * decceleration;

            if (velocityX < -currentMaxVelocity && velocityX > -currentMaxVelocity - 0.01f)
            {
                velocityX = -currentMaxVelocity;
            }
        }
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < -currentMaxVelocity - 0.01f)
        {
            velocityX = -currentMaxVelocity;
        }

        // Lock Right
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * decceleration;

            if (velocityX > currentMaxVelocity && velocityX < currentMaxVelocity + 0.01f)
            {
                velocityX = currentMaxVelocity;
            }
        }
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > currentMaxVelocity - 0.01f)
        {
            velocityX = currentMaxVelocity;
        }
    }
}
