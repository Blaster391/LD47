using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [RequireComponent(typeof(BoxCollider))]
    public class SwitchBehaviour : MonoBehaviour, IActivator
    {
        [SerializeField] private bool m_activate = false;

        [SerializeField] private MeshRenderer m_switchMesh = null;
        [SerializeField] private Material m_activatedMat = null;
        
        void Update()
        {
            // Manual testing
            if (m_activate)
            {
                Activate();
                m_activate = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Car")
            {
                Activate();
            }
        }

        // Public Assign
        public void AssignActivee(IActivatee i_activatee)
        {
            m_activatees.Add(i_activatee);
        }

        // Private Activation
        private void Activate()
        {
            if(m_switchMesh != null && m_activatedMat != null)
            {
                m_switchMesh.material = m_activatedMat;
            }

            ActivateActivatees();
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
