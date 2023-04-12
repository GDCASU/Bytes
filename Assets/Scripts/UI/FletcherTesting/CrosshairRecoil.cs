using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrosshairRecoil : MonoBehaviour
{
    private float radianAngle = (Mathf.PI / 4.0f); //Store radian angle for better efficiency (45°)
    //Recoil Vars
    public float recoilFloor = 9.0f; //minimum distance from center, FIXME: WHEN RECOIL SYSTEM IS FINISHED, MAKE IT READONLY
    public float recoilTEST;
    //Wings of the crosshair
    public Image wingTopRight;
    public Image wingTopLeft;
    public Image wingBottomRight;
    public Image wingBottomLeft;
    //Retrieve their RectTransforms, couldnt come up with better names soz :(
    private RectTransform wingUpRight;
    private RectTransform wingUpLeft;
    private RectTransform wingDownRight;
    private RectTransform wingDownLeft; 
    
    // Start is called before the first frame update
    void Start()
    {
        //Get the RectTransforms
        this.wingUpRight = wingTopRight.GetComponent<RectTransform>();
        this.wingUpLeft = wingTopLeft.GetComponent<RectTransform>();
        this.wingDownRight = wingBottomRight.GetComponent<RectTransform>();
        this.wingDownLeft = wingBottomLeft.GetComponent<RectTransform>(); 
        
        //Set the recoil temporarly
        recoilTEST = recoilFloor + 0.001f;
        //Set wings to the minimum amount of radius shift
        updatePositions(recoilTEST); //number makes sure that the "if" is entered
    }

    // Update is called once per frame
    void Update()
    {
        updatePositions(recoilTEST);
    }

    private void updatePositions(float recoil) 
    {
        if (recoil > recoilFloor)
        {
            float shift = calcShift(recoil);

            //Shift Top Right
            this.wingUpRight.anchoredPosition = new Vector3(+shift , +shift, 0);
            //Shift Top Left
            this.wingUpLeft.anchoredPosition = new Vector3(-shift , +shift, 0);
            //Shift Bottom Right
            this.wingDownRight.anchoredPosition = new Vector3(+shift , -shift, 0);
            //Shift Bottom Left
            this.wingDownLeft.anchoredPosition = new Vector3(-shift , -shift, 0);
        }
    }

    private float calcShift (float radius) 
    {
        //Since they are at a 45°, the X and Y shift are the same
        return radius * Mathf.Cos(this.radianAngle);
    }

}