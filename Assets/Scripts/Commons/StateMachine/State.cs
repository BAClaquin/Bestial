using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;



namespace StateMachine
{

    // Operation that can be called during a state
    public delegate void StateOperationDelegate<TStateEnum, TStateMachineWorker, TEventSystem>(IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> ai_internalStateMachine)
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker;

    public abstract class State<TStateEnum, TStateMachineWorker, TEventSystem>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker
    {
        #region Members
        public TStateEnum ID { get; private set; }
        #endregion


        #region Private Members
        protected IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> m_internalStateMachine;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for Abstract State
        /// </summary>
        /// <param name="ai_id">ID of the state</param>
        public State(TStateEnum ai_id, StateMachine<TStateEnum, TStateMachineWorker, TEventSystem> ai_stateMachine)
        {
            ID = ai_id;
            m_internalStateMachine = ai_stateMachine;
        }
        #endregion

        public State(TStateEnum ai_id)
        {
            ID = ai_id;
        }

        #region Public 

        public void setStateMachine(IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> ai_stateMachine)
        {
            m_internalStateMachine = ai_stateMachine;
        }

        /// <summary>
        /// Tells if the current state has the same ID as the other
        /// </summary>
        /// <param name="ai_other">other state to compare to</param>
        /// <returns>True if same ID, false otherwise</returns>
        public bool IsSameState(State<TStateEnum, TStateMachineWorker, TEventSystem> ai_other)
        {
            return ID.Equals(ai_other.ID);
        }

        /// <summary>
        /// State ID as a string
        /// </summary>
        /// <returns>result string</returns>
        public override string ToString()
        {
            return ID.ToString();
        }

        /// <summary>
        /// Is called when you enter the state for the first time
        /// </summary>
        public void OnEnter()
        {
            Tracer.Instance.Trace(TraceLevel.INFO2, "Entering state" + ToString());
            _onEnterImpl();
        }

        /// <summary>
        /// Is called when you are on the state
        /// </summary>
        public void OnState()
        {
            Tracer.Instance.Trace(TraceLevel.DEBUG, "On state" + ToString());
            _onStateImpl();
        }


        /// <summary>
        /// Is called when you leave the state
        /// </summary>
        public void OnLeave()
        {
            Tracer.Instance.Trace(TraceLevel.INFO2, "Leaving state " + ToString());
            _onLeaveImpl();
        }


        /// <summary>
        /// TODO : a la construction ????
        /// </summary>
        /// <param name="ai_internalStateMachine"></param>
        public void SetInternalStateMachine(IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem>  ai_internalStateMachine)
        {
            this.m_internalStateMachine = ai_internalStateMachine;
        }
        #endregion

        #region Protected Functions
        /// <summary>
        ///  State specific on state impl if required
        /// </summary>
        protected virtual void _onStateImpl() { }
        /// <summary>
        ///  State specific on enter impl if required
        /// </summary>
        protected virtual void _onEnterImpl() { }
        /// <summary>
        ///  State specific on leave impl if required
        /// </summary>
        protected virtual void _onLeaveImpl() { }
        #endregion
    }
}