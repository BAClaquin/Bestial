using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

namespace GameStateMachine
{
    public class Worker : StateMachine.IStateMachineWorker
    {

        public Unit m_currentUnit { get; set;}
        public Unit m_targetUnit { get; set;}
        public Tile m_targetTile { get; set;}

        public void reset()
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "Implement the reset");
            //throw new System.NotImplementedException();
        }

    }
}

