using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public abstract class AFactory<TStateEnum, TStateMachineWorker, TEventSystem>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker, new()
        where TEventSystem : new()
    {
        #region Private Members
        StateMachine<TStateEnum, TStateMachineWorker, TEventSystem> m_stateMachine;
        Configuration<TStateEnum, TStateMachineWorker, TEventSystem> m_config;
        TStateMachineWorker m_worker;
        IGame m_game;
        TEventSystem m_eventSystem;
        #endregion

        #region Constructors
        public AFactory(IGame ai_game)
        {
            m_worker = new TStateMachineWorker();
            m_game = ai_game;
            m_eventSystem = new TEventSystem();
            m_stateMachine = new StateMachine<TStateEnum, TStateMachineWorker, TEventSystem>(m_worker, m_eventSystem, m_game);
        }
        #endregion

        #region Utilitary Functions for creation
        public IStateMachine<TEventSystem> Create(string ai_stateMachineName)
        {
            // create new configuration
            m_config = new Configuration<TStateEnum, TStateMachineWorker, TEventSystem>(ai_stateMachineName);
            // user defined function adding states and transition
            CreateStatesAndTransitions();
            // setting the configuration to the state machine
            m_stateMachine.setConfiguration(m_config);

            // return created state machine
            return m_stateMachine;
        }
        /// <summary>
        /// Creates and adds a new state to the configuration 
        /// </summary>
        /// <param name="ai_stateEnumID">Id for the state</param>
        /// <param name="ai_isStartState">is this state the start state</param>
        /// <returns>the added state</returns>
        //protected State<TStateEnum, TStateMachineWorker, TEventSystem> AddNewState(TStateEnum ai_stateEnumID, bool ai_isStartState = false)
        //{
        //    return m_config.addState(new State<TStateEnum, TStateMachineWorker, TEventSystem>(ai_stateEnumID, m_stateMachine), ai_isStartState);
        //}

        protected void AddNewState(State<TStateEnum, TStateMachineWorker, TEventSystem> ai_state, bool ai_isStartState = false)
        {
            ai_state.SetInternalStateMachine(m_stateMachine);
            m_config.addState(ai_state, ai_isStartState);
        }

        /// <summary>
        /// Creates and add a new transition to the configuration
        /// </summary>
        /// <param name="ai_from">State ID from</param>
        /// <param name="ai_to">State ID to</param>
        /// <param name="ai_conditionFunction">Condition function for transiting</param>
        /// <returns>the added transition</returns>
        protected Transition<TStateEnum, TStateMachineWorker, TEventSystem> AddNewTransition(TStateEnum ai_from, TStateEnum ai_to, EvaluateConditionDelegate<TStateEnum, TStateMachineWorker, TEventSystem> ai_conditionFunction)
        {
            return m_config.addTransition(new Transition<TStateEnum, TStateMachineWorker, TEventSystem>(ai_from, ai_to, ai_conditionFunction, m_stateMachine));
        }
        #endregion

        #region Function to define for your own factory
        /// <summary>
        /// Here you will have to create and define states and transitions for your state machine
        /// Use addNewState and addNewTransition
        /// The for each state or transition configured, add the optional functions you want to set
        /// Once this function is implemented, you can call "Create" on your factory
        /// </summary>
        protected abstract void CreateStatesAndTransitions();
        #endregion
    }
}