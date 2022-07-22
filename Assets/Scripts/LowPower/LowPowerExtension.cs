﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public static class LowPowerExtension 
    {
        public static int ToInt(this System.Enum pEnum) => (int)(object)pEnum;
    }
}