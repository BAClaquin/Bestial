using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;

    public class Utils
    {
        public static bool tileIsInReach(IInternalStateMachine ai_internalStateMachine)
        {
            return ai_internalStateMachine.GetGame().tileIsInReachOfUnit(ai_internalStateMachine.GetWorker().m_currentUnit, ai_internalStateMachine.GetWorker().m_targetTile);
        }

        public static bool currentUnitIsPlayable(IInternalStateMachine ai_internalStateMachine)
        {
            return ai_internalStateMachine.GetGame().unitIsPlayable(ai_internalStateMachine.GetWorker().m_currentUnit);
        }        

        public static bool eventOccured(IInternalStateMachine ai_internalStateMachine, EventEnum ai_event)
        {
            return ai_internalStateMachine.GetEventSystem().HasEventOccured(ai_event);
        }

        public static bool unitIsAttackable(IInternalStateMachine ai_internalStateMachine)
        {
            return ai_internalStateMachine.GetGame().unitIsAttackableByUnit(ai_internalStateMachine.GetWorker().m_currentUnit, ai_internalStateMachine.GetWorker().m_currentUnit);
        }

        public static void displayPossibleActions(IInternalStateMachine ai_internalStateMachine)
        {
            ai_internalStateMachine.GetGame().highlightAccessibleTiles(ai_internalStateMachine.GetWorker().m_currentUnit);
        }

        public static void undisplayPossibleAction(IInternalStateMachine ai_internalStateMachine)
        {
            Tracer.print("undisplaypossibleactions");
            ai_internalStateMachine.GetGame().unHighlightAccessibleTiles();
        }

        public static void attackUnit(IInternalStateMachine ai_internalStateMachine)
        {
            ai_internalStateMachine.GetGame().attackEnnemi(ai_internalStateMachine.GetWorker().m_currentUnit, ai_internalStateMachine.GetWorker().m_targetUnit);
        }
    }
}
