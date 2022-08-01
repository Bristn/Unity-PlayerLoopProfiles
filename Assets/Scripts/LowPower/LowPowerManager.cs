using Assets.Scripts.LowPower.PlayerLoop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public class LowPowerManager
    {
        private static LowPowerManager instance;

        public static LowPowerManager Instance
        {
            get
            {
                if (instance == null || instance.Equals(null))
                {
                    instance = new LowPowerManager();
                }
                return instance;
            }
        }



        public LowPowerDispatcher dispatcher { get; private set; }


        public PlayerLoopManager playerLoopManager { get; private set; }

        public LowPowerManager()
        {
            dispatcher = new LowPowerDispatcher();

            playerLoopManager = new PlayerLoopManager();
        }
    }
}