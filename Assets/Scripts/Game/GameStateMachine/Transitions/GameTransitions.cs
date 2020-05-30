using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



namespace GameStateMachine
{
    using IInternalStateMachine = StateMachine.IInternalStateMachine<GameStates, GameWorker, IGameEventConsumer>;
    using GameTransitionBehaviour = StateMachine.TransitionBehaviour<GameStates, GameWorker, IGameEventConsumer>;

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
                return m_eventConsumer.UnitSelectedConsumer().HasOccured();
            }

            public override void ConsumeAndStore()
            {
                m_worker.LastSelectedUnit = m_eventConsumer.UnitSelectedConsumer().ConsumeAssociatedData();
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
                if (m_eventConsumer.UnitSelectedConsumer().HasOccured())
                {
                    var unit = m_eventConsumer.UnitSelectedConsumer().GetAssociatedData();
                    return m_game.UnitBelongsToCurrentPlayer(unit);
                }
                return false;
            }

            public override void ConsumeAndStore()
            {
                m_worker.LastSelectedUnit = m_eventConsumer.UnitSelectedConsumer().ConsumeAssociatedData();
            }

            public override void OnTransitionAction()
            {
                // if current unit has consumed some of its actions when leaving : disable it
                if (m_worker.CurrentUnit.HasConsumedActions())
                {
                    m_worker.CurrentUnit.Disable();
                }
            }
        }

        /// <summary>
        /// When an unit attackable by the CurrentUnit is selected
        /// </summary>
        public class Event_AttackableEnnemySelected : GameTransitionBehaviour
        {
            public Event_AttackableEnnemySelected(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }

            public override bool EvaluateCondition()
            {
                if (m_eventConsumer.UnitSelectedConsumer().HasOccured())
                {
                    var unit = m_eventConsumer.UnitSelectedConsumer().GetAssociatedData();
                    if (Utils.CanUnitAttack(m_game, m_worker.CurrentUnit, unit))
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }

            public override void ConsumeAndStore()
            {
                m_worker.LastSelectedUnit = m_eventConsumer.UnitSelectedConsumer().ConsumeAssociatedData();
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
                return m_game.UnitIsPlayable(m_worker.LastSelectedUnit);
            }

            public override void OnTransitionAction()
            {
                // set the semected unit as current one
                m_worker.SelectCurrentUnit(m_worker.LastSelectedUnit);
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
                return !m_game.UnitIsPlayable(m_worker.LastSelectedUnit);
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
                return !m_game.UnitIsPlayable(m_worker.CurrentUnit);
            }

            // when unit is not playable disable it
            public override void OnTransitionAction()
            {
                m_worker.CurrentUnit.Disable();
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
                if (m_eventConsumer.TileSelectedConsumer().HasOccured())
                {
                    // get info about event tile
                    var w_tile = m_eventConsumer.TileSelectedConsumer().GetAssociatedData();
                    // if the unit can move to this tile
                    return m_game.UnitCanMoveToTile(m_worker.CurrentUnit, w_tile);
                }

                return false;
            }

            public override void ConsumeAndStore()
            {
                m_worker.LastSelectedTile = m_eventConsumer.TileSelectedConsumer().ConsumeAssociatedData();
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
                return m_eventConsumer.MoveIsOverConsumer().HasOccured();
            }

            public override void ConsumeAndStore()
            {
                m_eventConsumer.MoveIsOverConsumer().Consume(); ;
            }
        }
        #endregion

        #region general Transitions

        /// <summary>
        /// When a nextTurn is requested by the player
        /// </summary>
        public class Event_NextTurnRequested : GameTransitionBehaviour
        {
            public Event_NextTurnRequested(IInternalStateMachine ai_internalStateMachine) : base(ai_internalStateMachine) { }

            public override bool EvaluateCondition()
            {
                return m_eventConsumer.NextTurnConsumer().HasOccured();
            }

            public override void ConsumeAndStore()
            {
                m_eventConsumer.NextTurnConsumer().Consume();
            }
        }

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