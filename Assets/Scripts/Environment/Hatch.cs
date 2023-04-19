using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hatch : Door
{
    public GameObject elevator;
    
    private bool isTopHatch;

    private void Start()
    {
        if (!elevator.activeSelf)
            isTopHatch = false;
        else
            isTopHatch = true;
    }
}
