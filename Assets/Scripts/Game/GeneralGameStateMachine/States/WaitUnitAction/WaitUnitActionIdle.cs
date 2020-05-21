/*
using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;

    public class WaitUnitActionIdle : GameState
    {

        public WaitUnitActionIdle() : base(GameStates.WAIT_UNIT_ACTION_IDLE) { }

        public static bool toWaitUnitActionUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {
            if (Utils.eventOccured(ai_internalStateMachine, EventEnum.UNIT_SELECTED))
            {
                ai_internalStateMachine.GetWorker().m_targetUnit = ai_internalStateMachine.GetEventSystem().ConsumeUnitSelectedEvent();
                return true;
            }
            return false;            
        }

        public static bool toWaitUnitActionTileSelected(IInternalStateMachine ai_internalStateMachine)
        {
            if (Utils.eventOccured(ai_internalStateMachine, EventEnum.TILE_SELECTED))
            {
                ai_internalStateMachine.GetWorker().m_targetTile = ai_internalStateMachine.GetEventSystem().ConsumeTileSelectedEvent();
                return true;
            }
            return false;
        }

        override
        protected void _onEnterImpl()
        {
            m_internalStateMachine.GetWorker().m_currentUnit.Highlight();
            Utils.displayPossibleActions(m_internalStateMachine);
        }

        override
        protected void _onLeaveImpl() => m_internalStateMachine.GetWorker().m_currentUnit.Unhighlight();


    }
}
*/