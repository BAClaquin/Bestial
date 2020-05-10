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

        public static bool toMoveUnit(IInternalStateMachine ai_internalStateMachine)
        {
            if (Utils.eventOccured(ai_internalStateMachine, EventEnum.TILE_SELECTED))
            {
                ai_internalStateMachine.GetWorker().m_targetTile = ai_internalStateMachine.GetEventSystem().getTileSelectedEventAssociatedData();
                return Utils.tileIsInReach(ai_internalStateMachine);
            }
            return false;
        }

        public static bool toWaitGameAction(IInternalStateMachine ai_internalStateMachine)
        {
            if(Utils.eventOccured(ai_internalStateMachine, EventEnum.TILE_SELECTED))
            {
                ai_internalStateMachine.GetWorker().m_targetTile = ai_internalStateMachine.GetEventSystem().getTileSelectedEventAssociatedData();
                return !Utils.tileIsInReach(ai_internalStateMachine);
            }
            return false;
        }

        public static bool toUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {            
            if (Utils.eventOccured(ai_internalStateMachine, EventEnum.UNIT_SELECTED))
            {
                ai_internalStateMachine.GetWorker().m_targetUnit = ai_internalStateMachine.GetEventSystem().getUnitSelectedEventAssociatedData();
                if (Utils.unitIsAttackable(ai_internalStateMachine))
                {
                    // unit not attackable => select unit = set targetunit as currentUnit
                    ai_internalStateMachine.GetWorker().m_currentUnit = ai_internalStateMachine.GetWorker().m_targetUnit;
                    return true;
                }
                return false;
            }
            return false;
        }

        public static bool toAttack(IInternalStateMachine ai_internalStateMachine)
        {            
            if(Utils.eventOccured(ai_internalStateMachine, EventEnum.UNIT_SELECTED))
            {
                ai_internalStateMachine.GetWorker().m_targetUnit = ai_internalStateMachine.GetEventSystem().getUnitSelectedEventAssociatedData();
                return Utils.unitIsAttackable(ai_internalStateMachine);                
            }
            return false;
        }

        override protected void _onEnterImpl()
        {
            Utils.displayPossibleActions(m_internalStateMachine);
        }

        override protected void _onLeaveImpl()
        {
            Utils.undisplayPossibleAction(m_internalStateMachine);
        }
    }
}
