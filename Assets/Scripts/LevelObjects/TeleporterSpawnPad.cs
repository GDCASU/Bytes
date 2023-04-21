using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterSpawnPad : SpawnPad
{
    const float TELEPORTER_OFFSET_Y = 1.5f;

    public GameObject teleporter;

    void Start()
    {
        type = PadType.Teleporter;
    }

    public void SpawnTeleporter()
    {
        Vector3 telPos = this.transform.position;
        telPos.y += TELEPORTER_OFFSET_Y;

        Instantiate(teleporter, telPos, Quaternion.identity, gameObject.transform);
        if (debug) Debug.Log("Boss Room Teleporter Spawned");
    }

    void OnDrawGizmos()
    {
        Vector3 gizmoPos = this.transform.position;
        gizmoPos.y += TELEPORTER_OFFSET_Y;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(gizmoPos, new Vector3(2, 3, 2));
    }
}
