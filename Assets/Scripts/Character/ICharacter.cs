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
    public Transform[] GetDetectionPoints();
    public void TakeDamage(float damage);
}
