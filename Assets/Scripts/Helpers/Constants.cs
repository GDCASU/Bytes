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
        public const string Protagonist = "Protagonist";
        public const string Antagonist = "Antagonist";
    }

    public static class Layer
    {
        public const int Environment = 0;
        public const int Character = 6;
        public const int Protagonist = 7;
        public const int Antagonist = 8;
        public const int Touchable = 9;
        public const int Interactable = 10;
    }

    public static class LayerMask
    {
        public const int Environment = 1 << Layer.Environment;
        public const int Character = 1 << Layer.Character;
        public const int Protagonist = 1 << Layer.Protagonist;
        public const int Antagonist = 1 << Layer.Antagonist;
        public const int Touchable = 1 << Layer.Touchable;
        public const int Interactable = 1 << Layer.Interactable;
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
