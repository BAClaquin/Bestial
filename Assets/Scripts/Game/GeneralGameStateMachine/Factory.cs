using Assets.Scripts.Game.GeneralGameStateMachine;
using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStateEnum, Worker, GameEventSystem>;

    class Factory : AFactory<GameStateEnum, Worker, GameEventSystem>
    {

        #region automate Functions

        public void onEnterUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {
            Tracer.Instance.Trace(TraceLevel.INFO1, "TODO : Display unit info");
        }

        #endregion

        #region automate Transition Functions

        public bool waitGameActionToUnitSelectedCondition(IInternalStateMachine ai_internalStateMachine)
        {
            if (ai_internalStateMachine.GetEventSystem().HasEventOccured(EventEnum.UNIT_SELECTED))
            {
                Unit unit = ai_internalStateMachine.GetEventSystem().ConsumeUnitSelectedEvent();
                ai_internalStateMachine.GetWorker().setCurrentUnit(unit);
                return true;
            }
            return false;
        }

        #endregion

        public Factory(IGame ai_game) : base(ai_game)
        {

        }

        protected override void CreateStatesAndTransitions()
        {
            //states
            AddNewState(GameStateEnum.WAIT_GAME_ACTION, true);
            var w_unitSelected = AddNewState(GameStateEnum.UNIT_SELECTED);
            w_unitSelected.SetOnEnterFuntion(onEnterUnitSelected);

            //transitions
            AddNewTransition(GameStateEnum.WAIT_GAME_ACTION, GameStateEnum.UNIT_SELECTED, waitGameActionToUnitSelectedCondition);
        }
    }

}

