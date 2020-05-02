using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Interface for state machine destinated for using it
/// </summary>
public interface IStateMachine
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
    State<StateEnum> getCurrentState();
}

/// <summary>
/// State Machine
/// </summary>
public class StateMachine<StateEnum> : IInternalStateMachine<StateEnum>, IStateMachine
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

    /// <summary>
    /// Is state machine started
    /// </summary>
    bool m_isStarted;

    /// <summary>
    /// State machine external metadata
    /// </summary>
    IStateMachineMetadata m_metadata;

    /// <summary>
    /// Eligible transitions for current state
    /// </summary>
    List<Transition<StateEnum>> m_eligibleTransitions;
    #endregion

    #region Constructors
    /// <summary>
    ///  Constructor for state machine
    /// </summary>
    /// <param name="ai_configuration">COndigfuration of states and transitions</param>
    /// <param name="ai_metadata">External metadata the state machine can work with</param>
    public StateMachine(StateMachineConfiguration<StateEnum> ai_configuration, IStateMachineMetadata ai_metadata)
    {
        m_configuration = ai_configuration;
        m_currentState = m_configuration.getStartState();
        m_isStarted = false;
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

    /// <summary>
    /// Starts the state machine
    /// sets the current state as starting state
    /// Resets statemachine metadata
    /// </summary>
    public void Start()
    {
        // preconditions
        if( ! m_configuration.checkConfiguration())
        {
            Tracer.Instance.Trace(TraceLevel.ERROR, "State Machine configuration is incorrect : won't start");
            return;
        }
        // start state machine
        m_isStarted = true;
        SetCurrentState(m_configuration.getStartState());
        m_metadata.reset();
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
        if(!m_isStarted) { return; }

        // call on state function for current state
        m_currentState.OnState();

        // see if one transition is possible
        Transition<StateEnum> w_transitionToExecute = LookForPossibleTransition(m_eligibleTransitions);
        // if not : end for this computeState
        if(w_transitionToExecute == null) { return; }

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
    private void SetCurrentState(State<StateEnum> ai_state)
    {
        m_currentState = ai_state;
        // execute state enter function
        m_currentState.OnEnter();
        // look for eleigible transitions for this state
        m_eligibleTransitions = m_configuration.getAllTransitionsFrom(m_currentState);

        Tracer.Instance.Trace(TraceLevel.INFO1, "Current state is " + m_currentState.ToString());
    }

    /// <summary>
    /// Test transitions conditions, and provides the first one with true response
    /// </summary>
    /// <param name="ai_eligibleTransitions">Transitions you want to test</param>
    /// <returns>A traistion if one foud, null otherwise</returns>
    private Transition<StateEnum> LookForPossibleTransition(List<Transition<StateEnum>> ai_eligibleTransitions)
    {
        foreach(var transition in ai_eligibleTransitions)
        {
            if(transition.evaluateCondition())
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
    private void ExecuteTransition(Transition<StateEnum> ai_transition)
    {
        // precondition
        if(m_currentState.IsSameState(ai_transition.From))
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "Atempting a transition from state " + ai_transition.From.ToString() + " when current state id " + m_currentState.ToString());
            return;
        }

        // execute state leave
        ai_transition.From.OnLeave();
        // execute transition action
        ai_transition.CallTransitionAction();
        // set the new reached state
        SetCurrentState(ai_transition.To);
    }
    #endregion
}
