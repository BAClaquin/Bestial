using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{

    public interface IStateMachineWorker
    {
        /// <summary>
        /// Resets all data to default value
        /// </summary>
        void reset();
    }

}
