using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class SwitchBehaviour : MonoBehaviour, IActivator
    {
        [SerializeField] private bool m_activate = false;

        void Update()
        {
            // IF CAR HITS VOLUME OR SOMIN
            if (m_activate)
            {
                if (m_activatee != null)
                {
                    m_activatee.Activate();
                }
                m_activate = false;
            }
        }

        public void AssignActivee(IActivatee i_activatee)
        {
            m_activatee = i_activatee;
        }

        private IActivatee m_activatee;
    }
}
