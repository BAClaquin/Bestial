﻿using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<StateEnum, Worker, EventSystem>;
    class WaitUnitActionTileSelected : StateImpl
    {

        public WaitUnitActionTileSelected() : base(StateEnum.WAIT_UNIT_ACTION_TILE_SELECTED) { }

        internal static bool toMoveUnit(IInternalStateMachine ai_internalStateMachine)
        {
            return Utils.tileIsInReach(ai_internalStateMachine);
        }

        internal static bool toWaitGameAction(IInternalStateMachine ai_internalStateMachine)
        {
            return !Utils.tileIsInReach(ai_internalStateMachine);
        }

        override
        public void OnLeave()
        {
            Utils.undisplayPossibleAction(m_internalStateMachine);
        }
    }
}
