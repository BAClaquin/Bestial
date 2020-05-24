using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace StateMachine
{

    public class Configuration<TStateEnum, TStateMachineWorker,TEventConsumer>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker
    {
        #region Members
        /// <summary>
        /// Name of the state machine
        /// </summary>
        public string StateMachineName { get; private set; }
        #endregion

        #region Private Members
        /// <summary>
        /// List of states
        /// </summary>
        private List<State<TStateEnum, TStateMachineWorker, TEventConsumer>> m_states;
        /// <summary>
        /// List of states
        /// </summary>
        private List<Transition<TStateEnum, TStateMachineWorker, TEventConsumer>> m_transitions;
        /// <summary>
        /// First state when statemachine is started
        /// </summary>
        State<TStateEnum, TStateMachineWorker, TEventConsumer> m_startState;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor for state machine configuration
        /// </summary>
        /// <param name="ai_stateMachineName">Name of the state_machine</param>
        public Configuration(string ai_stateMachineName)
        {
            StateMachineName = ai_stateMachineName;
            m_states = new List<State<TStateEnum, TStateMachineWorker, TEventConsumer>>();
            m_transitions = new List<Transition<TStateEnum, TStateMachineWorker, TEventConsumer>>();
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Adds a state to dthe configuration
        /// </summary>
        /// <param name="ai_state">State to add</param>
        /// <returns> The state juste added </returns>
        public State<TStateEnum, TStateMachineWorker, TEventConsumer> addState(State<TStateEnum, TStateMachineWorker, TEventConsumer> ai_state, bool ai_isStartState = false)
        {
            // check if states isn't already here
            foreach (var state in m_states)
            {
                if (state.IsSameState(ai_state))
                {
                    throw new System.Exception("State " + ai_state.ToString() + " already exists for this configuration");
                }
            }
            m_states.Add(ai_state);


            // set it ad first state if asked for
            if (ai_isStartState)
            {
                if (m_startState != null)
                {
                    throw new System.Exception("You are setting a new startState when one is already provided. This behaviour is forbidden.");
                }
                m_startState = ai_state;
            }

            // return the last added state
            return m_states[m_states.Count - 1];
        }

        /// <summary>
        /// Adds a transition to the list of possible transitions
        /// </summary>
        /// <param name="ai_transition">Transition to add</param>
        public Transition<TStateEnum, TStateMachineWorker, TEventConsumer> addTransition(Transition<TStateEnum, TStateMachineWorker, TEventConsumer> ai_transition)
        {
            // check if states isn't already here
            foreach (var transition in m_transitions)
            {
                if (transition.isSameTransition(ai_transition))
                {
                    throw new System.Exception("Transition " + ai_transition.ToString() + " already exists for this configuration");
                }
            }
            m_transitions.Add(ai_transition);

            // return the last added transition
            return m_transitions[m_transitions.Count - 1];
        }

        /// <summary>
        /// Provides the start state
        /// </summary>
        /// <returns>state</returns>
        public State<TStateEnum, TStateMachineWorker, TEventConsumer> getStartState()
        {
            if (m_startState == null)
            {
                throw new System.Exception("Start state not allocated");
            }
            return m_startState;
        }

        /// <summary>
        /// Test if state machine is correctly confifgured
        /// </summary>
        /// <returns></returns>
        public bool checkConfiguration()
        {
            // check start state defined
            if (m_startState == null)
            {
                Tracer.Instance.Trace(TraceLevel.ERROR, "No start state defined ! (use addState(state, true))");
                return false;
            }
            // all good
            return true;
        }

        /// <summary>
        /// Provides all transition starting from a specific state
        /// </summary>
        /// <param name="ai_state">The state you want tto start from</param>
        /// <returns>All transitions found</returns>
        public List<Transition<TStateEnum, TStateMachineWorker, TEventConsumer>> getAllTransitionsFrom(State<TStateEnum, TStateMachineWorker, TEventConsumer> ai_state)
        {
            List<Transition<TStateEnum, TStateMachineWorker, TEventConsumer>> w_result = new List<Transition<TStateEnum, TStateMachineWorker, TEventConsumer>>();

            // browse all transitions
            foreach (var transition in m_transitions)
            {
                // if the transitions starts from the desired state add it to result list
                if (transition.From.Equals(ai_state.ID))
                {
                    w_result.Add(transition);
                }
            }

            return w_result;
        }

        /// <summary>
        /// Finds a state matching and ID
        /// </summary>
        /// <param name="ai_id">ID you look for</param>
        /// <returns>Foudn result</returns>
        public State<TStateEnum, TStateMachineWorker, TEventConsumer> getStateByID(TStateEnum ai_id)
        {
            foreach (var state in m_states)
            {
                if (state.ID.Equals(ai_id))
                {
                    return state;
                }
            }

            throw new System.Exception("State with ID " + ai_id.ToString() + "not found in configurtaion.");
        }
        #endregion
    }

}
