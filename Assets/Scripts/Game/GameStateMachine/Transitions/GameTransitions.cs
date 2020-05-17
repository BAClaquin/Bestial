using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



namespace GameStateMachine
{
    using IInternalStateMachine = StateMachine.IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;
    using GameTransitionBehaviour = StateMachine.TransitionBehaviour<GameStates, GameWorker, GameEventSytstem>;

    namespace TransitionBehaviours
    {

        #region Unit Related Transitions
        /// <summary>
        /// When any unit is selected
        /// </summary>
        public class Event_AnyUnitSelected : GameTransitionBehaviour
        {
            public Event_AnyUnitSelected(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                return Utils.eventOccured(m_internalStateMachine, EventEnum.UNIT_SELECTED);
            }

            public override void ConsumeAndStore()
            {
                m_internalStateMachine.GetWorker().LastSelectedUnit = m_internalStateMachine.GetEventSystem().ConsumeUnitSelectedEvent();
            }
        }

        /// <summary>
        /// When an unit belonging to the current player is selected.
        /// </summary>
        public class Event_SelectedPlayerUnit : GameTransitionBehaviour
        {
            public Event_SelectedPlayerUnit(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                if(Utils.eventOccured(m_internalStateMachine, EventEnum.UNIT_SELECTED))
                {
                    var unit = m_internalStateMachine.GetEventSystem().GetUnitSelectedData();
                    return m_internalStateMachine.GetGame().UnitBelongsToCurrentPlayer(unit);
                }
                return false;
            }

            public override void ConsumeAndStore()
            {
                m_internalStateMachine.GetWorker().LastSelectedUnit = m_internalStateMachine.GetEventSystem().ConsumeUnitSelectedEvent();
            }

            public override void OnTransitionAction()
            {
                // if current unit has consumed some of its actions when leaving : disable it
                if(m_internalStateMachine.GetWorker().CurrentUnit.HasConsumedActions())
                {
                    m_internalStateMachine.GetWorker().CurrentUnit.Disable();
                }
            }
        }

        /// <summary>
        /// When an unit attackable by the CurrentUnit is selected
        /// </summary>
        public class Event_AttackableEnnemySelected: GameTransitionBehaviour
        {
            public Event_AttackableEnnemySelected(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }

            public override bool EvaluateCondition()
            {
                if (Utils.eventOccured(m_internalStateMachine, EventEnum.UNIT_SELECTED))
                {
                    var unit = m_internalStateMachine.GetEventSystem().GetUnitSelectedData();
                    if( Utils.CanUnitAttack(m_internalStateMachine, m_internalStateMachine.GetWorker().CurrentUnit, unit) )
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }

            public override void ConsumeAndStore()
            {
                m_internalStateMachine.GetWorker().LastSelectedUnit = m_internalStateMachine.GetEventSystem().ConsumeUnitSelectedEvent();
            }
        }

        /// <summary>
        /// Guard for LastSelectedUnit to be playable by the current player
        /// </summary>
        public class Guard_SelectedUnitIsPlayable : GameTransitionBehaviour
        {
            public Guard_SelectedUnitIsPlayable(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                return m_internalStateMachine.GetGame().UnitIsPlayable(m_internalStateMachine.GetWorker().LastSelectedUnit);
            }

            public override void OnTransitionAction()
            {
                // set the semected unit as current one
                m_internalStateMachine.GetWorker().SelectCurrentUnit(m_internalStateMachine.GetWorker().LastSelectedUnit);
            }
        }

        /// <summary>
        /// Guard for LastSelectedUnit to be playable by the current player
        /// </summary>
        public class Guard_SelectedUnitIsNotPlayable : GameTransitionBehaviour
        {
            public Guard_SelectedUnitIsNotPlayable(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                return ! m_internalStateMachine.GetGame().UnitIsPlayable(m_internalStateMachine.GetWorker().LastSelectedUnit);
            }
        }

        /// <summary>
        /// Guard for CurrentUnit not playable by current player
        /// </summary>
        public class Guard_CurrentUnitIsNotPlayable : GameTransitionBehaviour
        {
            public Guard_CurrentUnitIsNotPlayable(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                return !m_internalStateMachine.GetGame().UnitIsPlayable(m_internalStateMachine.GetWorker().CurrentUnit);
            }

            // when unit is not playable disable it
            public override void OnTransitionAction()
            {
                m_internalStateMachine.GetWorker().CurrentUnit.Disable();
            }
        }
        #endregion

        #region Tile Related Transitions
        /// <summary>
        /// When any unit is selected
        /// </summary>
        public class Event_AccessibleTileSelected : GameTransitionBehaviour
        {
            public Event_AccessibleTileSelected(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                if( Utils.eventOccured(m_internalStateMachine, EventEnum.TILE_SELECTED) )
                {
                    // get info about event tile
                    var w_tile = m_internalStateMachine.GetEventSystem().GetTileSelectedData();
                    // if the unit can move to this tile
                    return Utils.CurrentUnitCanMoveTo(m_internalStateMachine, w_tile);
                }

                return false;
            }

            public override void ConsumeAndStore()
            {
                m_internalStateMachine.GetWorker().LastSelectedTile = m_internalStateMachine.GetEventSystem().ConsumeTileSelectedEvent();
            }
        }
        #endregion

        #region Actions Related Transitions
        /// <summary>
        /// When any unit is selected
        /// </summary>
        public class Event_MoveIsOver : GameTransitionBehaviour
        {
            public Event_MoveIsOver(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                return Utils.eventOccured(m_internalStateMachine, EventEnum.MOVE_IS_OVER);
            }

            public override void ConsumeAndStore()
            {
                m_internalStateMachine.GetEventSystem().ConsumeMoveOver();
            }
        }
        #endregion

        #region general Transitions

        /// <summary>
        /// Transition will always occur
        /// </summary>
        public class Guard_None : GameTransitionBehaviour
        {
            public Guard_None(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }
            public override bool EvaluateCondition()
            {
                return true;
            }
        }
        #endregion
    }
}