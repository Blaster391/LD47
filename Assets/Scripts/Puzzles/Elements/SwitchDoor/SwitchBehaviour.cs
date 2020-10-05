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
                ActivateActivatees();
                m_activate = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Car")
            {
                ActivateActivatees();
            }
        }

        public void AssignActivee(IActivatee i_activatee)
        {
            m_activatees.Add(i_activatee);
        }

        private void ActivateActivatees()
        {
            foreach(IActivatee activatee in m_activatees)
            {
                activatee.Activate();
            }
        }

        private List<IActivatee> m_activatees = new List<IActivatee>();
    }
}
