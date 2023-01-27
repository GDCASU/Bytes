using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] Player _player;

    public float mouseSensitvity = 100f;
    public float camHeight = .75f;
    public Transform player;

    public Transform horizontalRotationHelper;
    float horizontalAngularVelocity;
    float verticalAngularVelocity;
    public float smoothTime = .02f;

    private bool canRotate;

    float xRotationHelper;

    private void Start()
    {
        transform.localRotation = player.transform.rotation;

        if (transform.parent) transform.parent = null;
        transform.localRotation = player.localRotation;

        horizontalRotationHelper.parent = null;
        horizontalRotationHelper.localRotation = transform.localRotation;
        xRotationHelper = transform.eulerAngles.x;

        canRotate = true;
    }

    private void LateUpdate()
    {
        horizontalRotationHelper.position = transform.position;
        transform.position = player.position + player.up * camHeight;

        if (canRotate)
        {
            Vector2 lookVector = _player.LookVector;
            float mouseX = lookVector.x * mouseSensitvity * Time.deltaTime;
            float mouseY = lookVector.y * mouseSensitvity * Time.deltaTime;

            mouseX = HorizontalRotation(mouseX);
            mouseY = VerticalRotation(mouseY);

            transform.localRotation = Quaternion.Euler(mouseY, mouseX, 0f);
        }
        else
            transform.localRotation = player.localRotation;
    }
    public void ToggleRotation(bool value) => canRotate = value;
    public void RestartRotation()
    {
        horizontalRotationHelper.localRotation = transform.localRotation;
        xRotationHelper = transform.eulerAngles.x;
        canRotate = true;
    }
    public float HorizontalRotation(float mouseX)
    {
        horizontalRotationHelper.Rotate(Vector3.up * mouseX, Space.Self);
        float angle = Mathf.SmoothDampAngle(
            transform.localEulerAngles.y, horizontalRotationHelper.localEulerAngles.y, ref horizontalAngularVelocity, smoothTime);
        return angle;
    }
    public float VerticalRotation(float mouseY)
    {
        xRotationHelper = Mathf.Clamp(xRotationHelper - mouseY, -90, 90);
        float angle = Mathf.SmoothDampAngle(
            transform.localEulerAngles.x, xRotationHelper, ref verticalAngularVelocity, smoothTime);
        return angle;
    }

    public void AdjustCameraHeight(bool moveDown, float cameraDisplacement)
    {
        if (moveDown) camHeight -= cameraDisplacement;
        else camHeight += cameraDisplacement;
    }
}