using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LowPower
{
    public class LowPowerMono : MonoBehaviour
    {
        public List<LowPowerImplementation> Implementations { set; private get; }

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (Implementations == null)
            {
                return;
            }

            Implementations.ForEach(imp => imp.Clean());
        }


        private void OnApplicationFocus(bool pFocused)
        {
            if (Implementations == null)
            {
                return;
            }

            Implementations.ForEach(imp => {
                if (pFocused)
                {
                    imp.Resume();
                }
                else
                {
                    imp.Pause();
                }
            });
        }

        private void OnApplicationPause(bool pPaused)
        {
            if (Implementations == null)
            {
                return;
            }

            Implementations.ForEach(imp => {
                if (pPaused)
                {
                    imp.Pause();
                }
                else
                {
                    imp.Resume();
                }
            });
        }
    }
 }
 