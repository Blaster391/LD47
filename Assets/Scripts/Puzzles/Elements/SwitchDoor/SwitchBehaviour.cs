using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [RequireComponent(typeof(BoxCollider))]
    public class SwitchBehaviour : MonoBehaviour, IActivator
    {
        [SerializeField] private bool m_activate = false;
        
        void Update()
        {
            // Manual testing
            if (m_activate)
            {
                if (m_activatee != null)
                {
                    m_activatee.Activate();
                }
                m_activate = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Car")
            {
                m_activatee.Activate();
            }
        }

        public void AssignActivee(IActivatee i_activatee)
        {
            m_activatee = i_activatee;
        }

        private IActivatee m_activatee;
    }
}
