using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameStateMachine
{

    class Factory : AFactory<StateEnum, Worker, EventSystem>
    {

        public Factory(IGame ai_game) : base(ai_game) { }

        protected override void CreateStatesAndTransitions()
        {
            AddStates();
            AddTransitions();            
        }

        #region private methods
        private void AddStates()
        {
            AddNewState(new WaitGameAction(), true);
            AddNewState(new UnitSelected());
            AddNewState(new WaitUnitAction());
            AddNewState(new WaitUnitActionIdle());
            AddNewState(new WaitUnitActionUnitSelected());
            AddNewState(new WaitUnitActionTileSelected());
            AddNewState(new MoveUnit());
            AddNewState(new Attack());
            AddNewState(new NextTurn());
        }

        private void AddTransitions()
        {
            AddWaitGameActionTransitions();
            AddUnitSelectedTransitions();
            AddWaitUnitActionTransitions();
            AddMoveUnitTransitions();
            AddAttackTransitions();            
        }

        private void AddWaitGameActionTransitions()
        {
            AddNewTransition(StateEnum.WAIT_GAME_ACTION, StateEnum.UNIT_SELECTED, WaitGameAction.toUnitSelected);
        }

        private void AddUnitSelectedTransitions()
        {
            AddNewTransition(StateEnum.UNIT_SELECTED, StateEnum.WAIT_GAME_ACTION, UnitSelected.toWaitGameAction);
            AddNewTransition(StateEnum.UNIT_SELECTED, StateEnum.WAIT_UNIT_ACTION_IDLE, UnitSelected.toWaitUnitActionIdle);
        }

        private void AddWaitUnitActionTransitions()
        {

            //// TILE SELECTED
            //AddNewTransition(StateEnum.WAIT_UNIT_ACTION, StateEnum.WAIT_GAME_ACTION, WaitUnitAction.toWaitGameAction);
            //AddNewTransition(StateEnum.WAIT_UNIT_ACTION, StateEnum.MOVE_UNIT, WaitUnitAction.toMoveUnit);
            //// UNIT SELECTED
            //AddNewTransition(StateEnum.WAIT_UNIT_ACTION, StateEnum.UNIT_SELECTED, WaitUnitAction.toUnitSelected);
            //AddNewTransition(StateEnum.WAIT_UNIT_ACTION, StateEnum.ATTACK, WaitUnitAction.toAttack);

            //IDLE
            AddNewTransition(StateEnum.WAIT_UNIT_ACTION_IDLE, StateEnum.WAIT_UNIT_ACTION_UNIT_SELECTED, WaitUnitActionIdle.toWaitUnitActionUnitSelected);
            AddNewTransition(StateEnum.WAIT_UNIT_ACTION_IDLE, StateEnum.WAIT_UNIT_ACTION_TILE_SELECTED, WaitUnitActionIdle.toWaitUnitActionTileSelected);

            //TILE SELECTED
            AddNewTransition(StateEnum.WAIT_UNIT_ACTION_TILE_SELECTED, StateEnum.WAIT_GAME_ACTION, WaitUnitActionTileSelected.toWaitGameAction);
            AddNewTransition(StateEnum.WAIT_UNIT_ACTION_TILE_SELECTED, StateEnum.MOVE_UNIT, WaitUnitActionTileSelected.toMoveUnit);

            //UNIT SELECTED
            AddNewTransition(StateEnum.WAIT_UNIT_ACTION_UNIT_SELECTED, StateEnum.UNIT_SELECTED, WaitUnitActionUnitSelected.toUnitSelected);
            AddNewTransition(StateEnum.WAIT_UNIT_ACTION_UNIT_SELECTED, StateEnum.ATTACK, WaitUnitActionUnitSelected.toAttack);
        }

        private void AddMoveUnitTransitions()
        {
            AddNewTransition(StateEnum.MOVE_UNIT, StateEnum.UNIT_SELECTED, MoveUnit.toUnitSelected);
        }

        private void AddAttackTransitions()
        {
            AddNewTransition(StateEnum.ATTACK, StateEnum.UNIT_SELECTED, Attack.toUnitSelected);
        }

        #endregion

    }

}

