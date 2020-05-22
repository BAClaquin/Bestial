using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace StateMachine
{
    /// <summary>
    /// Interface for state machine destinated for using it
    /// </summary>
    public interface IStateMachine<TEventEmiter>
    {
        /// <summary>
        /// Starts the state machine
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the state machine
        /// </summary>
        void Stop();

        /// <summary>
        /// Computes states and transition for the state machine
        /// </summary>
        void ComputeState();

        /// <summary>
        /// Provides access to event system
        /// </summary>
        /// <returns>State machine event system</returns>
        TEventEmiter GetEventEmiter();
    }

    /// <summary>
    /// Interface destinated to members of the state machine to acces state machine data
    /// Provides acces to all data necessary for states and transition to operate
    /// </summary>
    public interface IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventConsumer>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker
    {
        /// <summary>
        /// Provides current state of the stateMachine
        /// </summary>
        /// <returns>The current state</returns>
        State<TStateEnum, TStateMachineWorker,TEventConsumer> GetCurrentState();

        /// <summary>
        /// Provides access to event system
        /// </summary>
        /// <returns>State machine event system</returns>
        TEventConsumer GetEventConsumer();

        /// <summary>
        /// Provies acces to state machien worker
        /// </summary>
        /// <returns>StateMachine worker</returns>
        TStateMachineWorker GetWorker();

        /// <summary>
        /// Provides acces to IGame
        /// </summary>
        /// <returns></returns>
        IGame GetGame();
    }



    /// <summary>
    /// State Machine
    /// </summary>
    public class StateMachine<TStateEnum, TStateMachineWorker, TEventConsumer, TEventEmiter> : IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventConsumer >, IStateMachine<TEventEmiter>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker
    {
        #region Private Members
        /// <summary>
        /// Current state of the state machine
        /// </summary>
        State<TStateEnum, TStateMachineWorker,TEventConsumer> m_currentState;

        /// <summary>
        /// Configuration of states and transition for the state machine
        /// </summary>
        Configuration<TStateEnum, TStateMachineWorker, TEventConsumer> m_configuration;

        /// <summary>
        /// Is state machine started
        /// </summary>
        bool m_isStarted;

        /// <summary>
        /// State machine worker class
        /// </summary>
        TStateMachineWorker m_worker;

        /// <summary>
        /// Eligible transitions for current state
        /// </summary>
        List<Transition<TStateEnum, TStateMachineWorker, TEventConsumer>> m_eligibleTransitions;

        /// <summary>
        /// Event system for the state machine : for consumer
        /// </summary>
        TEventConsumer m_eventConsumer;

        /// <summary>
        /// Event system for the state machine : for consumer
        /// </summary>
        TEventEmiter m_eventEmiter;

        /// <summary>
        /// Base event system to consume all events
        /// </summary>
        BaseEventSystem m_eventSystem;

        /// <summary>
        /// Game interface
        /// </summary>
        IGame m_game;
        #endregion

        #region Constructors
        /// <summary>
        ///  Constructor for state machine
        /// </summary>
        /// <param name="ai_configuration">COndigfuration of states and transitions</param>
        /// <param name="ai_worker">External worker the state machine can work with</param>
        public StateMachine(TStateMachineWorker ai_worker, BaseEventSystem ai_eventSystem, TEventConsumer ai__eventConsumer,  TEventEmiter ai_eventEmiter, IGame ai_game)
        {
            m_isStarted = false;
            m_worker = ai_worker;
            m_eventConsumer = ai__eventConsumer;
            m_eventEmiter = ai_eventEmiter;
            m_eventSystem = ai_eventSystem;
            m_game = ai_game;
        }
        #endregion

        #region Public Functions
        public void setConfiguration(Configuration<TStateEnum, TStateMachineWorker, TEventConsumer> ai_configuration)
        {
            m_configuration = ai_configuration;
            m_currentState = m_configuration.getStartState();
        }

        /// <summary>
        /// Provides current state of the stateMachine
        /// </summary>
        /// <returns>The current state</returns>
        public State<TStateEnum, TStateMachineWorker, TEventConsumer> GetCurrentState()
        {
            return m_currentState;
        }

        /// <summary>
        /// Starts the state machine
        /// sets the current state as starting state
        /// Resets statemachine metadata
        /// </summary>
        public void Start()
        {
            Tracer.Instance.Trace(TraceLevel.INFO1, "{"  + m_configuration.StateMachineName + "}" + " : STARTING...");
            m_worker.reset();
            // preconditions
            if (!m_configuration.checkConfiguration())
            {
                Tracer.Instance.Trace(TraceLevel.ERROR, "State machine configuration is incorrect : won't start");
                return;
            }
            SetCurrentState(m_configuration.getStartState());
            // start state machine
            m_isStarted = true;
            Tracer.Instance.Trace(TraceLevel.INFO1, "{"  + m_configuration.StateMachineName + "} : STARTED !");
        }

        /// <summary>
        /// Stops the staemachine
        /// </summary>
        public void Stop()
        {
            m_isStarted = false;
        }

        /// <summary>
        /// Computes the state of the state machine
        /// Based on states and transitions
        /// Will executes associated actions
        /// </summary>
        public void ComputeState()
        {
            // only allows computation when statemachine is started
            if (!m_isStarted) { return; }

            // call on state function for current state
            m_currentState.OnState();

            // see if one transition is possible
            Transition<TStateEnum, TStateMachineWorker, TEventConsumer> w_transitionToExecute = LookForPossibleTransition(m_eligibleTransitions);
            // if not : end for this computeState
            if (w_transitionToExecute == null)
            {
                // reject all raised events that were not consumed.
                m_eventSystem.RejectAllEvents();
                return;
            }

            // we found a transition execute it
            ExecuteTransition(w_transitionToExecute);
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Sets the new current state
        /// Calls onEnter function for this state
        /// </summary>
        /// <param name="ai_state">Current state to set</param>
        private void SetCurrentState(State<TStateEnum, TStateMachineWorker, TEventConsumer> ai_state)
        {
            m_currentState = ai_state;
            // execute state enter function
            m_currentState.OnEnter();
            // update eleigible transitions
            m_eligibleTransitions = m_configuration.getAllTransitionsFrom(m_currentState);

            Tracer.Instance.Trace(TraceLevel.INFO1, "{" + m_configuration.StateMachineName + "} state is " + m_currentState.ToString());
        }

        /// <summary>
        /// Test transitions conditions, and provides the first one with true response
        /// </summary>
        /// <param name="ai_eligibleTransitions">Transitions you want to test</param>
        /// <returns>A traistion if one foud, null otherwise</returns>
        private Transition<TStateEnum, TStateMachineWorker, TEventConsumer> LookForPossibleTransition(List<Transition<TStateEnum, TStateMachineWorker, TEventConsumer>> ai_eligibleTransitions)
        {
            foreach (var transition in ai_eligibleTransitions)
            {
                if (transition.EvaluateCondition())
                {
                    return transition;
                }
            }

            return null;
        }

        /// <summary>
        /// Executes a complete transition
        /// From state leave action is called
        /// Transition actions is called
        /// Enters new current state with OnEnter action called
        /// </summary>
        /// <param name="ai_transition">The transition to execute</param>
        private void ExecuteTransition(Transition<TStateEnum, TStateMachineWorker, TEventConsumer> ai_transition)
        {
            // precondition
            if (!m_currentState.ID.Equals(ai_transition.From))
            {
                Tracer.Instance.Trace(TraceLevel.WARNING, "Atempting a transition from state " + ai_transition.From.ToString() + " when current state id " + m_currentState.ToString());
                return;
            }

            // execute state leave
            m_configuration.getStateByID(ai_transition.From).OnLeave();
            // execute transition action
            ai_transition.ExecuteTransition();
            // set the new reached state
            SetCurrentState(m_configuration.getStateByID(ai_transition.To));
        }

        /// <summary>
        /// Provides acces to state machine event system for Consumer
        /// </summary>
        /// <returns></returns>
        public TEventConsumer GetEventConsumer()
        {
            return m_eventConsumer;
        }

        /// <summary>
        /// Provides acces to state machine event system for Emiter
        /// </summary>
        /// <returns></returns>
        public  TEventEmiter GetEventEmiter()
        {
            return m_eventEmiter;
        }

        public TStateMachineWorker GetWorker()
        {
            return m_worker;
        }

        public IGame GetGame()
        {
            return m_game;
        }
        #endregion
    }
}