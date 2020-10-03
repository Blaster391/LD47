using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class DoorBehaviour : MonoBehaviour, IActivatee
    {
        [SerializeField] private GameObject m_gateLeftObject = null;
        [SerializeField] private GameObject m_gateRightObject = null;

        public void Activate()
        {
            // Open the gate
            // Animate or something eventually
            if (m_gateLeftObject != null)
            {
                Destroy(m_gateLeftObject);
            }
            if(m_gateRightObject != null)
            {
                Destroy(m_gateRightObject);
            }
        }
    }
}
