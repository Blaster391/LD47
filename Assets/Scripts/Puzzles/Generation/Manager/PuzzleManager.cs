using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    /*
     *  The famed Puzzle Manager, what mysteries doth thine hold.
     *  
     *  
     */
    public class PuzzleManager : MonoBehaviour
    {
        [SerializeField] private Spline m_spline;

        void Start()
        {
            if(m_spline == null)
            {
                Debug.LogError("No Spline Referenced in Puzzle Manager Script");
            }

            // Cache some values
        }

        void Update()
        {

        }


        // State
        float m_runningDifficulty = 0f;
        float m_runningDistance = 0f;
        float m_trackLengthWS = 0f;
        int m_trackLengthPS = 0; // Puzzle Space boiiii

    }
}
