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


        private static Queue<Action<int>> events = new Queue<Action<int>>();
        private static Queue<int> parameters = new Queue<int>();

        public static void DispatchEvent(Action<int> pEvent, int pValue)
        {
            lock (events)
            {
                events.Enqueue(pEvent);
                parameters.Enqueue(pValue);
            }
        }

        public static void Update()
        {
            if (!Application.isPlaying)
            {
                return; // TODO
            }

            lock (events)
            {
                while (events.Count > 0)
                {
                    events.Dequeue().Invoke(parameters.Dequeue());
                }
            }

            LowPowerTimeout.UpdateTimeout();
        }
    }
}