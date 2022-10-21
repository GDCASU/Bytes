/*
 * Author: Cristion Dominguez
 * Date: 6 Oct. 2022
 */

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

    public CharacterType Target
    {
        get
        {
            if (Type == CharacterType.Player)
                return CharacterType.Enemy;
            else
                return CharacterType.Player;
        }
    }

    protected virtual void OnEnable()
    {
        Health = MaxHealth;
    }

    public abstract void ReceiveDamage(float damage);
    public abstract void ReceiveHealth(float health);
}

public enum CharacterType : int
{
    Player,
    Enemy
}

public static class CharacterTypeExtensions
{
    public static int GetLayer(this CharacterType type)
    {
        if (type == CharacterType.Player)
            return Constants.Layer.Protagonist;
        else
            return Constants.Layer.Antagonist;
    }

    public static int GetLayerMask(this CharacterType type)
    {
        if (type == CharacterType.Player)
            return Constants.LayerMask.Protagonist;
        else
            return Constants.LayerMask.Antagonist;
    }
}