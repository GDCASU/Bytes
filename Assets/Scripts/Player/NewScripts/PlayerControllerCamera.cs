using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControllerNew
{
    [Header("Camera")]
    [SerializeField] Transform cameraTrans;
    [SerializeField] Transform cameraHolderTrans;
    [SerializeField] Transform cameraPosition;
    [SerializeField] float xSens;
    [SerializeField] float ySens;

    float xRotation;
    float yRotation;

    private void handleCamera()
    {
        //rotate the camera
        Vector2 mouseInput = _input.LookVector;
        float mouseX = mouseInput.x * Time.deltaTime * xSens;
        float mouseY = mouseInput.y * Time.deltaTime * ySens;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTrans.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        //move the camera with the player
        cameraHolderTrans.position = cameraPosition.position;
    }


}
