using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.LowLevel;

namespace Assets.Scripts.LowPower
{
    /// <summary>
    /// A custom update class which isn't removed from the player loop at any point
    /// </summary>
    public static class LowPowerDispatcher
    {
        public static PlayerLoopSystem dispatchSystem { get; private set; } = new PlayerLoopSystem()
        {
            type = typeof(LowPowerDispatcher),
            updateDelegate = Update,
        };


        public static void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            LowPowerTimeout.UpdateTimeout();
        }
    }
}