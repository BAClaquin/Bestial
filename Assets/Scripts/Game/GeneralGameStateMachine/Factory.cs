using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameStateMachine
{

    class Factory : AFactory<GameStates, GameWorker, GameEventSytstem>
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
            AddNewTransition(GameStates.WAIT_GAME_ACTION, GameStates.UNIT_SELECTED, WaitGameAction.toUnitSelected);
        }

        private void AddUnitSelectedTransitions()
        {
            AddNewTransition(GameStates.UNIT_SELECTED, GameStates.WAIT_GAME_ACTION, UnitSelected.toWaitGameAction);
            AddNewTransition(GameStates.UNIT_SELECTED, GameStates.WAIT_UNIT_ACTION_IDLE, UnitSelected.toWaitUnitActionIdle);
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
            AddNewTransition(GameStates.WAIT_UNIT_ACTION_IDLE, GameStates.WAIT_UNIT_ACTION_UNIT_SELECTED, WaitUnitActionIdle.toWaitUnitActionUnitSelected);
            AddNewTransition(GameStates.WAIT_UNIT_ACTION_IDLE, GameStates.WAIT_UNIT_ACTION_TILE_SELECTED, WaitUnitActionIdle.toWaitUnitActionTileSelected);

            //TILE SELECTED
            AddNewTransition(GameStates.WAIT_UNIT_ACTION_TILE_SELECTED, GameStates.WAIT_GAME_ACTION, WaitUnitActionTileSelected.toWaitGameAction);
            AddNewTransition(GameStates.WAIT_UNIT_ACTION_TILE_SELECTED, GameStates.MOVE_UNIT, WaitUnitActionTileSelected.toMoveUnit);

            //UNIT SELECTED
            AddNewTransition(GameStates.WAIT_UNIT_ACTION_UNIT_SELECTED, GameStates.UNIT_SELECTED, WaitUnitActionUnitSelected.toUnitSelected);
            AddNewTransition(GameStates.WAIT_UNIT_ACTION_UNIT_SELECTED, GameStates.ATTACK, WaitUnitActionUnitSelected.toAttack);
        }

        private void AddMoveUnitTransitions()
        {
            AddNewTransition(GameStates.MOVE_UNIT, GameStates.UNIT_SELECTED, MoveUnit.toUnitSelected);
        }

        private void AddAttackTransitions()
        {
            AddNewTransition(GameStates.ATTACK, GameStates.UNIT_SELECTED, Attack.toUnitSelected);
        }

        #endregion

    }

}

