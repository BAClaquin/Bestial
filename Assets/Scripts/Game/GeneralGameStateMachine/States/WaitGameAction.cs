using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<StateEnum, Worker, EventSystem>;

    public class WaitGameAction : StateImpl
    {
        public WaitGameAction(): base(StateEnum.WAIT_GAME_ACTION) {}

        public static bool toUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {
            if (Utils.eventOccured(ai_internalStateMachine, EventEnum.UNIT_SELECTED))
            {
                ai_internalStateMachine.GetWorker().m_currentUnit = ai_internalStateMachine.GetEventSystem().ConsumeUnitSelectedEvent();
                return true;
            }
            return false;
        }

    }
}
