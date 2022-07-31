using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public static class LowPowerExtension 
    {
        public static int ToInt(this System.Enum pEnum) => (int)(object)pEnum;

        /*
        public static bool IsTemporary(this InteractionType pType)
        {
            return pType == InteractionType.DSEKTOP_MOUSE_MOVE || pType == InteractionType.DSEKTOP_MOUSE_SCROLL;
        }
        */
    }
}