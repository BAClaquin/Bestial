using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameStateMachine;
using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<StateEnum, Worker, EventSystem>;
    public class MoveUnit : StateImpl
    {
        public MoveUnit() : base(StateEnum.MOVE_UNIT) { }

        public static bool toUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {
            // quit state when moving animation is over
            return Utils.eventOccured(ai_internalStateMachine, EventEnum.ANIMATION_IS_OVER);
        }

        //private IEnumerator moveUnit(IInternalStateMachine ai_internalStateMachine)
        //{
        //    // move current unit to target tile
        //    yield return ai_internalStateMachine.GetGame().moveUnitToTile(ai_internalStateMachine.GetWorker().m_currentUnit, ai_internalStateMachine.GetWorker().m_targetTile);
          
        //}

        override
        public void OnEnter()
        {
            m_internalStateMachine.GetGame().moveUnitToTile(m_internalStateMachine.GetWorker().m_currentUnit, m_internalStateMachine.GetWorker().m_targetTile);
        }

    }
}
