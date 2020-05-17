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
        public static bool CurrentUnitCanMoveTo(IInternalStateMachine ai_internalStateMachine, Tile ai_tile)
        {
            // unit can move and tile is in reach of unit
            var w_unit = ai_internalStateMachine.GetWorker().CurrentUnit;
            return ai_internalStateMachine.GetGame().UnitCanMoveToTile(w_unit, ai_tile);
        }


        public static bool eventOccured(IInternalStateMachine ai_internalStateMachine, EventEnum ai_event)
        {
            return ai_internalStateMachine.GetEventSystem().HasEventOccured(ai_event);
        }

        public static bool CanUnitAttack(IInternalStateMachine ai_internalStateMachine, Unit ai_attacker, Unit ai_attackee)
        {
            if(ai_attacker.CanAttack())
            {
                return ai_internalStateMachine.GetGame().UnitIsAttackableByUnit(ai_attacker, ai_attackee);
            }
            return false;
        }

        public static void DisplayPossibleActions(IInternalStateMachine ai_internalStateMachine, Unit ai_unit)
        {
            Tracer.Instance.Trace(TraceLevel.INFO2, "Possible Move : " + ai_unit.CanMove());
            if (ai_unit.CanMove())
            {
                ai_internalStateMachine.GetGame().HighlightAccessibleTiles(ai_unit);
            }
            Tracer.Instance.Trace(TraceLevel.INFO2, "Possible Attack : " + ai_unit.CanAttack());
            if (ai_unit.CanAttack())
            {
                ai_internalStateMachine.GetGame().DisplayAvailableTargets(ai_unit);
            }
        }

        public static void undisplayPossibleAction(IInternalStateMachine ai_internalStateMachine)
        {
            ai_internalStateMachine.GetGame().UnlightAllActions();
        }

        public static void attackUnit(IInternalStateMachine ai_internalStateMachine)
        {
            ai_internalStateMachine.GetGame().AttackEnnemi(ai_internalStateMachine.GetWorker().CurrentUnit, ai_internalStateMachine.GetWorker().LastSelectedUnit);
        }
    }
}
