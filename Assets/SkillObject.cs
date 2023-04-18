using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillObject : MonoBehaviour
{
    //Remember, the keyword  for an abstract in c# is virtual

    public string skillName;
    public float chargeCost;
    //public Image icon;

    //Overloaded constructor
    public SkillObject(string skillName, float chargeCost)
    {
        this.skillName = skillName;
        this.chargeCost = chargeCost;
    }

    //Public constructor for empty objects
    public SkillObject()
    {
        this.skillName = null;
        this.chargeCost = null;
    }

    //FIXME: FIND A WAY OF INITIALIZING AN IMAGE

    public bool isEmpty()
    {
        if (skillName == null && chargeCost == null)
        {
            return true; //Skill is empty
        }
        return false;
    }
    
    public void setName(string name) 
    {
        this.skillName = name;
    }

    public void setChargeCost(float cost)
    {
        this.chargeCost = cost;
    }

    public float getName()
    {
        return this.skillName;
    }

    public float getCost()
    {
        return this.chargeCost;
    }
    
}
