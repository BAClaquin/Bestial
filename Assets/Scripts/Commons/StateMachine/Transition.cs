using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{

    public delegate bool EvaluateConditionDelegate<TStateEnum, TStateMachineWorker, TEventSystem>(IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> ai_internalStateMachine)
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker;

    public delegate void OnTransitionDelegate<TStateEnum, TStateMachineWorker, TEventSystem>(IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> ai_internalStateMachine)
                where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker;

    public class Transition<TStateEnum, TStateMachineWorker, TEventSystem>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker
    {
        #region Members
        /// <summary>
        /// State to transit from
        /// </summary>
        public TStateEnum From { get; private set; }
        /// <summary>
        /// State to transit to
        /// </summary>
        public TStateEnum To { get; private set; }
        #endregion

        #region Private Members
        IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> m_internalStateMachine;
        TransitionBehaviour<TStateEnum, TStateMachineWorker, TEventSystem> m_transitionBehaviour;
        #endregion

        #region Constructors
        public Transition(TStateEnum ai_from, TStateEnum ai_to,
            TransitionBehaviour<TStateEnum, TStateMachineWorker, TEventSystem> ai_transitionBehaviour,
            IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> ai_stateMachine)
        {
            From = ai_from;
            To = ai_to;
            m_transitionBehaviour = ai_transitionBehaviour;
            m_internalStateMachine = ai_stateMachine;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Tells is this transition is the same (FROM and TO) than the other provided one
        /// </summary>
        /// <param name="ai_other">other transition you wanna test equality for</param>
        /// <returns>True if FROM and TO are the same, false otherwise</returns>
        public bool isSameTransition(Transition<TStateEnum, TStateMachineWorker, TEventSystem> ai_other)
        {
            return From.Equals(ai_other.From) && To.Equals(ai_other.To);
        }

        /// <summary>
        /// Describes transition as a string
        /// </summary>
        /// <returns>Result of description</returns>
        public override string ToString()
        {
            return "{Transition from " + From.ToString() + " to " + To.ToString() + "}";
        }


        public bool EvaluateCondition()
        {
            return m_transitionBehaviour.EvaluateCondition();
        }

        public void ExecuteTransition()
        {
            m_transitionBehaviour.ExecuteTransition();
        }
        #endregion
    }
}