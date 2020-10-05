using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class DoorBehaviour : MonoBehaviour, IActivatee
    {
        [SerializeField] private GameObject m_bollardsObject = null;
        [SerializeField] private float m_timeToOpen = 0.2f;

        bool m_lower = false;

        float m_startingYScale = 1f;

        private void Start()
        {
            m_startingYScale = m_bollardsObject.transform.localScale.y;
        }

        public void Activate()
        {
            m_lower = true;
        }

        private void Update()
        {
            if(m_lower && m_bollardsObject.transform.localScale.y != 0)
            {
                Vector3 currentScale = m_bollardsObject.transform.localScale;
                currentScale.y = Mathf.Max(0f, currentScale.y - (Time.deltaTime / m_timeToOpen * m_startingYScale));
                m_bollardsObject.transform.localScale = currentScale;
            }
        }
    }
}
