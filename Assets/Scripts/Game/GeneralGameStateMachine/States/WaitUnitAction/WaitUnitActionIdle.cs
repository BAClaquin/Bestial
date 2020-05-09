using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<StateEnum, Worker, EventSystem>;

    public class WaitUnitActionIdle : StateImpl
    {

        public WaitUnitActionIdle() : base(StateEnum.WAIT_UNIT_ACTION_IDLE) { }

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
        public void OnEnter()
        {
            Utils.displayPossibleActions(m_internalStateMachine);
        }


    }
}
