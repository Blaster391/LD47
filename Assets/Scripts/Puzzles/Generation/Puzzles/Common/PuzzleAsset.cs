using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  The base abstract class of puzzle assets of a certain puzzle type. 
 *  Lets us author different properties as we go in an easy way to help with testing puzzle generation.
 *  Also allows us to add new puzzles in an easy way making sure they provide a base set of values we use to pick a puzzle.
 */

namespace Puzzles
{
    public abstract class PuzzleAsset : ScriptableObject, IPuzzle
    {
        // Editable scriptable object params
        [SerializeField] private float m_difficultyMin = 0f;
        [SerializeField] private float m_difficultyMax = 1f;
        [SerializeField] private float m_visibilityMin = 0f;
        [SerializeField] private float m_visibilityMax = 1f;
        [SerializeField] private float m_probability = 1f;

        // Interface
        public float DifficultyMin => m_difficultyMin;
        public float DifficultyMax => m_difficultyMax;
        public float VisibilityMin => m_visibilityMin;
        public float VisibilityMax => m_visibilityMax;
        public float Probability => m_probability;
        public abstract IPuzzleGenerator Generator { get; }

        public System.Action ValuesChanged = delegate {};

        public void OnValidate()
        {
            ValuesChanged();
        }
    }
}
