using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrosshairSpread : MonoBehaviour
{
    //Trigonometry and debugging
    readonly private float cos45 = Mathf.Sqrt(2.0f) / 2.0f; //Store trigonometric result for better efficiency
    public bool ENABLE_spreadTEST = false; //Makes spreadTest change the crosshair
    public float spreadTEST;

    //spread Vars
    public float spreadFloor = 9.0f; //minimum distance from center, FIXME: WHEN spread SYSTEM IS FINISHED, MAKE IT READONLY
    
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
    
    void Start()
    {
        //Get the RectTransforms
        this.wingUpRight = wingTopRight.GetComponent<RectTransform>();
        this.wingUpLeft = wingTopLeft.GetComponent<RectTransform>();
        this.wingDownRight = wingBottomRight.GetComponent<RectTransform>();
        this.wingDownLeft = wingBottomLeft.GetComponent<RectTransform>(); 
        
        //Set the spread temporarly
        spreadTEST = spreadFloor + 0.001f;
        //Set wings to the minimum amount of radius shift
        updatePositions(spreadTEST); //number makes sure that the "if" is entered
    }

    void Update()
    {
        if (ENABLE_spreadTEST)
        {
            //Enables testing the crosshair with slider 
            this.updatePositions(spreadTEST); 
            //When weapons are finished, improve spread float retrieval
        }
    }

    public void spreadChange(float inputspread)
    {
        this.updatePositions(inputspread);
    }
    
    private void updatePositions(float spread) 
    {
        if (spread > spreadFloor)
        {
            float shift = calcShift(spread);

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
        return radius * this.cos45;
    }

}