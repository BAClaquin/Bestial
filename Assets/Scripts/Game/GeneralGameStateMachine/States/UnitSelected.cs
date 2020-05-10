using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;

    public class UnitSelected : GameState
    {
        public UnitSelected(): base(GameStates.UNIT_SELECTED) {}

        #region transitions

        public static bool toWaitGameAction(IInternalStateMachine ai_internalStateMachine)
        {
            if (!Utils.currentUnitIsPlayable(ai_internalStateMachine))
            {
                ai_internalStateMachine.GetWorker().m_currentUnit.Disable();
                return true;
            }
            
            return false;
        }

        public static bool toWaitUnitActionIdle(IInternalStateMachine ai_internalStateMachine)
        {
            return Utils.currentUnitIsPlayable(ai_internalStateMachine);
        }

        #endregion
    }
}
