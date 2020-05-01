using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for state machine destinated for using it
/// </summary>
public interface IStateMachine
{

}

/// <summary>
/// Interface destinated to members of the state machine to acces state machine data
/// </summary>
public interface IInternalStateMachine<StateEnum>
    where StateEnum : System.Enum
{
    /// <summary>
    /// Provides current state of the stateMachine
    /// </summary>
    /// <returns>The current state</returns>
    public State<StateEnum> getCurrentState();
}

/// <summary>
/// State Machine
/// </summary>
public class StateMachine<StateEnum> : IInternalStateMachine<StateEnum>
    where StateEnum : System.Enum
{
    #region Private Members
    /// <summary>
    /// Current state of the state machine
    /// </summary>
    State<StateEnum> m_currentState;

    /// <summary>
    /// Configuration of states and transition for the state machine
    /// </summary>
    StateMachineConfiguration<StateEnum> m_configuration;
    #endregion

    #region Constructors
    /// <summary>
    /// 
    /// </summary>
    public StateMachine(StateMachineConfiguration<StateEnum> ai_configuration)
    {
        m_configuration = ai_configuration;
        m_currentState = m_configuration.getStartState();
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Provides current state of the stateMachine
    /// </summary>
    /// <returns>The current state</returns>
    public State<StateEnum> getCurrentState()
    {
        return m_currentState;
    }
    #endregion
}
