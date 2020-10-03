using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    /*
     *  Other half of puzzle elements, the thing that gets activated
     */
    public interface IActivatee
    {
        void Activate();
    }
}
