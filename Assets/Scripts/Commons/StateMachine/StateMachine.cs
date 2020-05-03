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
public interface IInternalStateMachine<TStateEnum, TStateMachineWorker>
    where TStateEnum : System.Enum
    where TStateMachineWorker : IStateMachineWorker
{
    /// <summary>
    /// Provides current state of the stateMachine
    /// </summary>
    /// <returns>The current state</returns>
    State<TStateEnum, TStateMachineWorker> getCurrentState();
}

/// <summary>
/// State Machine
/// </summary>
public class StateMachine<TStateEnum, TStateMachineWorker> : IInternalStateMachine<TStateEnum, TStateMachineWorker>, IStateMachine
    where TStateEnum : System.Enum
    where TStateMachineWorker : IStateMachineWorker
{
    #region Private Members
    /// <summary>
    /// Current state of the state machine
    /// </summary>
    State<TStateEnum, TStateMachineWorker> m_currentState;

    /// <summary>
    /// Configuration of states and transition for the state machine
    /// </summary>
    StateMachineConfiguration<TStateEnum,TStateMachineWorker> m_configuration;

    /// <summary>
    /// Is state machine started
    /// </summary>
    bool m_isStarted;

    /// <summary>
    /// State machine external metadata
    /// </summary>
    IStateMachineWorker m_worker;

    /// <summary>
    /// Eligible transitions for current state
    /// </summary>
    List<Transition<TStateEnum, TStateMachineWorker>> m_eligibleTransitions;
    #endregion

    #region Constructors
    /// <summary>
    ///  Constructor for state machine
    /// </summary>
    /// <param name="ai_configuration">COndigfuration of states and transitions</param>
    /// <param name="ai_worker">External worker the state machine can work with</param>
    public StateMachine(TStateMachineWorker ai_worker)
    {

        m_currentState = m_configuration.getStartState();
        m_isStarted = false;
        m_worker = ai_worker;
    }
    #endregion

    #region Public Functions
    public void setConfiguration(StateMachineConfiguration<TStateEnum, TStateMachineWorker> ai_configuration)
    {
        m_configuration = ai_configuration;
    }

    /// <summary>
    /// Provides current state of the stateMachine
    /// </summary>
    /// <returns>The current state</returns>
    public State<TStateEnum, TStateMachineWorker> getCurrentState()
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
        m_worker.reset();
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
        Transition<TStateEnum, TStateMachineWorker> w_transitionToExecute = LookForPossibleTransition(m_eligibleTransitions);
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
    private void SetCurrentState(State<TStateEnum, TStateMachineWorker> ai_state)
    {
        m_currentState = ai_state;
        // execute state enter function
        m_currentState.OnEnter();
        // update eleigible transitions
        m_eligibleTransitions = m_configuration.getAllTransitionsFrom(m_currentState);

        Tracer.Instance.Trace(TraceLevel.INFO1, "{"+m_configuration.StateMachineName + "} Current state is " + m_currentState.ToString());
    }

    /// <summary>
    /// Test transitions conditions, and provides the first one with true response
    /// </summary>
    /// <param name="ai_eligibleTransitions">Transitions you want to test</param>
    /// <returns>A traistion if one foud, null otherwise</returns>
    private Transition<TStateEnum, TStateMachineWorker> LookForPossibleTransition(List<Transition<TStateEnum, TStateMachineWorker>> ai_eligibleTransitions)
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
    private void ExecuteTransition(Transition<TStateEnum, TStateMachineWorker> ai_transition)
    {
        // precondition
        if(m_currentState.ID.Equals(ai_transition.From))
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "Atempting a transition from state " + ai_transition.From.ToString() + " when current state id " + m_currentState.ToString());
            return;
        }

        // execute state leave
        m_configuration.getStateByID(ai_transition.From).OnLeave();
        // execute transition action
        ai_transition.CallTransitionAction();
        // set the new reached state
        SetCurrentState( m_configuration.getStateByID(ai_transition.To) );
    }
    #endregion
}
