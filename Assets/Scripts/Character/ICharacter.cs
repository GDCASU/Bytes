using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType: int
{
    Player = 3,
    Enemy = 6
}

public interface ICharacter
{
    public void ReceiveDamage(float damage);
    public void ReceiveHealth(float addedHealth);
    public Transform[] GetDetectionPoints();
}
