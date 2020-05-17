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

            AddNewState(new MoveUnit());
            AddNewState(new Attack());
            //AddNewState(new NextTurn());
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
            GameStates w_baseState = GameStates.WAIT_GAME_ACTION;
            // any unit selected
            AddNewTransition(w_baseState, GameStates.UNIT_SELECTED, new TransitionBehaviours.Event_AnyUnitSelected(m_stateMachine));
        }

        private void AddUnitSelectedTransitions()
        {
            GameStates w_baseState = GameStates.UNIT_SELECTED;
            // selected unit playable
            AddNewTransition(w_baseState, GameStates.WAIT_UNIT_ACTION, new TransitionBehaviours.Guard_SelectedUnitIsPlayable(m_stateMachine));
            // selected unit NOT playable
            AddNewTransition(w_baseState, GameStates.WAIT_GAME_ACTION, new TransitionBehaviours.Guard_SelectedUnitIsNotPlayable(m_stateMachine));
        }

        private void AddWaitUnitActionTransitions()
        {
            GameStates w_baseState = GameStates.WAIT_UNIT_ACTION;
            // move to tile
            AddNewTransition(w_baseState, GameStates.MOVE_UNIT, new TransitionBehaviours.Event_AccessibleTileSelected(m_stateMachine));
            // current unit is not playable
            AddNewTransition(w_baseState, GameStates.WAIT_GAME_ACTION, new TransitionBehaviours.Guard_CurrentUnitIsNotPlayable(m_stateMachine));
            // selected an other unit
            AddNewTransition(w_baseState, GameStates.UNIT_SELECTED, new TransitionBehaviours.Event_SelectedPlayerUnit(m_stateMachine));
            // attack an unit
            AddNewTransition(w_baseState, GameStates.ATTACK, new TransitionBehaviours.Event_AttackableEnnemySelected(m_stateMachine));
        }

        private void AddMoveUnitTransitions()
        {
            GameStates w_baseState = GameStates.MOVE_UNIT;
            // move to tile
            AddNewTransition(w_baseState, GameStates.WAIT_UNIT_ACTION, new TransitionBehaviours.Event_MoveIsOver(m_stateMachine));
        }

        private void AddAttackTransitions()
        {
            GameStates w_baseState = GameStates.ATTACK;
            // automaticly after entering attack --> leave to WAITunit
            AddNewTransition(w_baseState, GameStates.WAIT_UNIT_ACTION, new TransitionBehaviours.Guard_None(m_stateMachine));
        }
        #endregion

    }

}

