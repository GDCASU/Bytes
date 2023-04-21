using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hatch : Door
{
    public GameObject elevator;
    
    public bool isTopHatch;

    private void Start()
    {
        if (!elevator.activeSelf)
            isTopHatch = false;
        else
            isTopHatch = true;

        if (isTopHatch)
        {
            if (room.roomState == RoomState.Combat)
                elevator.SetActive(false);
            else
                elevator.SetActive(true);
        }
    }

    private void ToggleElevator(bool toggle)
    {
        if (isTopHatch)
        {
            if (toggle)
                elevator.SetActive(true);
            else
                elevator.SetActive(false);
        }
    }
}
