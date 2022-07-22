using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Assets.Scripts.LowPower
{
    /// <summary>
    /// A custom update class which isn't removed from the player loop at any point
    /// </summary>
    public class LowPowerDispatcher
    {
        public static PlayerLoopSystem dispatchSystem { get; private set; }

        private Queue<Action<int>> events = new Queue<Action<int>>();
        private Queue<int> parameters = new Queue<int>();
        private LowPowerTimeout timeout;

        public LowPowerDispatcher(LowPowerTimeout pTimeout)
        {
            timeout = pTimeout;

            PlayerLoopSystem system = new PlayerLoopSystem();
            system.type = typeof(LowPowerDispatcher);
            system.updateDelegate = Update;
            dispatchSystem = system;
        }


        public void DispatchEvent(Action<int> pEvent, int pValue)
        {
            lock (events)
            {
                events.Enqueue(pEvent);
                parameters.Enqueue(pValue);
            }
        }

        public void Update()
        {
            lock (events)
            {
                while (events.Count > 0)
                {
                    events.Dequeue().Invoke(parameters.Dequeue());
                }
            }

            timeout.UpdateTimeout();
        }
    }
}