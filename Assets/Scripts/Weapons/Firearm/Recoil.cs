using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    //rotations
    private Vector3 currRotation;
    private Vector3 targetRotation;

    //recoils
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    //Settings
    [Header("Recoil Settings")]
    [SerializeField] private float snapFactor;
    [SerializeField] private float returnSpeed;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Current Rotatoin: " + transform.rotation.ToString()) ;

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currRotation = Vector3.Slerp(currRotation, targetRotation, snapFactor * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currRotation);
        //Debug.Log("Recoil Rotation: " + transform.localRotation.ToString());
    }

    public void recoilFire() {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

}
