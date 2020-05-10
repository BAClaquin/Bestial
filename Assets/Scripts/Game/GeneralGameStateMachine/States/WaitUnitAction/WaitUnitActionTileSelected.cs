using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;
    class WaitUnitActionTileSelected : GameState
    {

        public WaitUnitActionTileSelected() : base(GameStates.WAIT_UNIT_ACTION_TILE_SELECTED) { }

        internal static bool toMoveUnit(IInternalStateMachine ai_internalStateMachine)
        {
            return Utils.tileIsInReach(ai_internalStateMachine);
        }

        internal static bool toWaitGameAction(IInternalStateMachine ai_internalStateMachine)
        {
            return !Utils.tileIsInReach(ai_internalStateMachine);
        }

        override
        protected void _onLeaveImpl()
        {
            Utils.undisplayPossibleAction(m_internalStateMachine);
        }
    }
}
