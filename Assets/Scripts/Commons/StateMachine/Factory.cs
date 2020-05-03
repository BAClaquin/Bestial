using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AStateMachineFactory<TStateEnum, TStateMachineWorker>
    where TStateEnum : System.Enum
    where TStateMachineWorker : IStateMachineWorker
{
    #region Private Members
    StateMachine<TStateEnum, TStateMachineWorker> m_stateMachine;
    StateMachineConfiguration<TStateEnum, TStateMachineWorker> m_config;
    TStateMachineWorker m_worker;
    IGame m_game;
    #endregion

    #region Constructors
    public AStateMachineFactory(TStateMachineWorker ai_worker, IGame ai_game)
    {
        m_worker = ai_worker;
        m_game = ai_game;
        m_stateMachine = new StateMachine<TStateEnum, TStateMachineWorker>(m_worker);
    }
    #endregion

    #region Utilitary Functions for creation
    public IStateMachine Create(string ai_stateMachineName)
    {
        // create new configuration
        m_config = new StateMachineConfiguration<TStateEnum, TStateMachineWorker>(ai_stateMachineName);
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
    State<TStateEnum, TStateMachineWorker> addNewState(TStateEnum ai_stateEnumID, bool ai_isStartState = false)
    {
        return m_config.addState(  new State<TStateEnum,TStateMachineWorker>(ai_stateEnumID, m_stateMachine, m_game, m_worker) ,   ai_isStartState);
    }

    /// <summary>
    /// Creates and add a new transition to the configuration
    /// </summary>
    /// <param name="ai_from">State ID from</param>
    /// <param name="ai_to">State ID to</param>
    /// <param name="ai_conditionFunction">Condition function for transiting</param>
    /// <returns>the added transition</returns>
    Transition<TStateEnum, TStateMachineWorker> addNewTransition(TStateEnum ai_from, TStateEnum ai_to, EvaluateConditionDelegate<TStateMachineWorker> ai_conditionFunction)
    {
        return m_config.addTransition(new Transition<TStateEnum, TStateMachineWorker>(ai_from, ai_to, ai_conditionFunction, m_stateMachine, m_game, m_worker));
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