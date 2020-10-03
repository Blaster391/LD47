using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  One half of the puzzle elements, the thing that the player interacts with in order to activate something else
 */

namespace Puzzles
{
    public interface IActivator
    {
        void AssignActivee(IActivatee i_activatee);
    }
}
