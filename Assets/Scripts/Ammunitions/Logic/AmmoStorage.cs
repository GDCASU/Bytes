using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStorage : MonoBehaviour
{
    [Header("Start Ammo")]
    [SerializeField] int laStart = 0;
    [SerializeField] int maStart = 0;
    [SerializeField] int haStart = 0;

    [Header("Max Ammo")]
    [SerializeField] int laMax = 500;
    [SerializeField] int maMax = 500;
    [SerializeField] int haMax = 500;

    [Header("AmmoStored")]
    [SerializeField] int lightAmmo = 0;
    [SerializeField] int mediumAmmo = 0;
    [SerializeField] int heavyAmmo = 0;

    // Start is called before the first frame update
    void Start()
    {
        lightAmmo = laStart;
        mediumAmmo = maStart;
        heavyAmmo = haStart;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int lightReloaded(int magSize, int remainingInMag)
    {
        lightAmmo += remainingInMag;

        if (lightAmmo >= magSize)
        {
            lightAmmo -= magSize;
            return magSize;
        }
        else
        {
            int remainingAmmo = lightAmmo;
            lightAmmo = 0;
            return remainingAmmo;
        }
    }

    public int mediumReloaded(int magSize, int remainingInMag)
    {
        mediumAmmo += remainingInMag;

        if (mediumAmmo >= magSize)
        {
            mediumAmmo -= magSize;
            return magSize;
        }
        else
        {
            int remainingAmmo = mediumAmmo;
            mediumAmmo = 0;
            return remainingAmmo;
        }
    }

    public int heavyReloaded(int magSize, int remainingInMag)
    {
        heavyAmmo += remainingInMag;

        if (heavyAmmo >= magSize)
        {
            heavyAmmo -= magSize;
            return magSize;
        }
        else
        {
            int remainingAmmo = heavyAmmo;
            heavyAmmo = 0;
            return remainingAmmo;
        }
    }
}
