using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace GameStateMachine
{
    public class Worker : StateMachine.IStateMachineWorker
    {

        private Unit m_currentUnit;

        public void reset()
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "Implement the reset");
            //throw new System.NotImplementedException();
        }

        public void setCurrentUnit(Unit ai_currentUnit)
        {
            this.m_currentUnit = ai_currentUnit;
        }
    }
}

