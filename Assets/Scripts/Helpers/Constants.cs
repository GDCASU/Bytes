using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Constants
{
    public static class Math
    {
        public const float PI_2 = Mathf.PI * 2f;
    }

    public static class Tags
    {
        public const string Player = "Player";
        public const string Enemy = "Enemy";
    }

    public static class Layer
    {
        public const int Environment = 0;
        public const int Player = 6;
        public const int Enemy = 7;
        public const int Projectile = 8;
        public const int EquipableEntity = 9;
    }

    public static class LayerMask
    {
        public const int Environment = 1 << Layer.Environment;
        public const int Player = 1 << Layer.Player;
        public const int Enemy = 1 << Layer.Enemy;
        public const int Projectile = 1 << Layer.Projectile;
        public const int EquipableEntity = 1 << Layer.EquipableEntity;
    }

    public class WaitFor
    {
        public static readonly WaitForEndOfFrame endOfFrame;
        public static readonly WaitForFixedUpdate fixedUpdate;
        
        static WaitFor()
        {
            endOfFrame = new WaitForEndOfFrame();
            fixedUpdate = new WaitForFixedUpdate();
        }
    }
}
