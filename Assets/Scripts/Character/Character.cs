using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: Header("Basic Properties")]
    [field: SerializeField]
    public CharacterType Type { get; protected set; }
    [field: SerializeField]
    public float MaxHealth { get; protected set; }
    [field: SerializeField]
    public Transform[] DetectionPoints { get; protected set; }

    [field: Header("Read-Only"), SerializeField]
    public float Health { get; protected set; }

    protected virtual void OnEnable()
    {
        Health = MaxHealth;
    }

    public abstract void ReceiveDamage(float damage);
    public abstract void ReceiveHealth(float health);
}

public enum CharacterType: int
{
    Player,
    Enemy
}

public static class CharacterTypeExtensions
{
    public static int GetLayer(this CharacterType type)
    {
        if (type == CharacterType.Player)
            return Constants.Layer.Player;
        else
            return Constants.Layer.Enemy;
    }

    public static int GetLayerMask(this CharacterType type)
    {
        if (type == CharacterType.Player)
            return Constants.LayerMask.Player;
        else
            return Constants.LayerMask.Enemy;
    }
}
