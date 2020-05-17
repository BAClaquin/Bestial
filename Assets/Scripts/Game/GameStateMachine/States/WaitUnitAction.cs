using GameStateMachine;
using StateMachine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;
    public class WaitUnitAction : GameState
    {
        public WaitUnitAction() : base(GameStates.WAIT_UNIT_ACTION) { }

        override protected void OnEnterImpl()
        {
            // displays possible actions for current unit
            Utils.DisplayPossibleActions(m_internalStateMachine, m_internalStateMachine.GetWorker().CurrentUnit);
        }

        override protected void OnLeaveImpl()
        {
            m_internalStateMachine.GetWorker().DeselectCurrentUnit();
            Utils.undisplayPossibleAction(m_internalStateMachine);
        }
    }
}
