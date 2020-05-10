using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameStateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;
    public class MoveUnit : GameStateMachine.GameState
    {
        public MoveUnit() : base(GameStates.MOVE_UNIT) { }

        public static bool toUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {
            // quit state when moving animation is over
            return Utils.eventOccured(ai_internalStateMachine, EventEnum.ANIMATION_IS_OVER);
        }

        override protected void _onEnterImpl()
        {
            m_internalStateMachine.GetGame().moveUnitToTile(m_internalStateMachine.GetWorker().m_currentUnit, m_internalStateMachine.GetWorker().m_targetTile);
        }

    }
}
