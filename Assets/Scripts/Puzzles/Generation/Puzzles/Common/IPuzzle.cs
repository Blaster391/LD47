using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * General interface for a 'puzzle' which is a non-physical entity that represents our idea of a puzzle.
 * It's agnostic of the type of puzzle because it will return general GameObjects or generic Interfaces.
 * Provides the requirements of this type of puzzle so it can be selected when appropriate
 * 
 * This will be
 * 
 * 
 * 
 *  So really I need a separation between a few things
 *  
 *      - Fixed values that are used to determin which puzzle to select
 *        These will be in a Scriptable object asset.
 *        It will implement an IPuzzle interface.
 *        That will provide access to an IPuzzleGenerator object
 *        
 *      - Actually generating the puzzle structure, its GameObjects, the links, etc.
 *        
 * 
 * 
 */

namespace Puzzles
{
    public interface IPuzzle
    {
        float DifficultyMin { get; }
        float DifficultyMax { get; }

        float VisibilityMin { get; }
        float VisibilityMax { get; }

        IPuzzleGenerator Generator { get; }
    }
}
