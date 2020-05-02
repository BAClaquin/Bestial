using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class StateMachineConfiguration<StateEnum>
    where StateEnum : System.Enum
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
    private List< State<StateEnum> > m_states;
    /// <summary>
    /// List of states
    /// </summary>
    private List< Transition<StateEnum> > m_transitions;
    /// <summary>
    /// First state when statemachine is started
    /// </summary>
    State<StateEnum> m_startState;
    #endregion


    #region Constructors
    /// <summary>
    /// Constructor for state machine configuration
    /// </summary>
    /// <param name="ai_stateMachineName">Name of the state_machine</param>
    public StateMachineConfiguration(string ai_stateMachineName)
    {
        StateMachineName = ai_stateMachineName;
        m_states = new List<State<StateEnum>>();
        m_transitions = new List<Transition<StateEnum>>();
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Adds a state to dthe configuration
    /// </summary>
    /// <param name="ai_state">State to add</param>
    public void addState(State<StateEnum> ai_state, bool ai_isStartState = false)
    {
        // check if states isn't already here
        foreach(var state in m_states)
        {
            if ( state.IsSameState( ai_state ) )
            {
                throw new System.Exception("State " + ai_state.ToString() + " already exists for this configuration");
            }
        }
        m_states.Add(ai_state);


        // set it ad first state if asked for
        if(ai_isStartState)
        {
            if(m_startState != null)
            {
                throw new System.Exception("You are setting a new startState when one is already provided. This behaviour is forbidden.");
            }
            m_startState = ai_state;
        }
    }

    /// <summary>
    /// Adds a transition to the list of possible transitions
    /// </summary>
    /// <param name="ai_transition">Transition to add</param>
    public void addTransition(Transition<StateEnum> ai_transition)
    {
        // check if states isn't already here
        foreach (var transition in m_transitions)
        {
            if ( transition.isSameTransition(ai_transition) )
            {
                throw new System.Exception("Transition " + ai_transition.ToString() + " already exists for this configuration");
            }
        }
        m_transitions.Add(ai_transition);
    }

    /// <summary>
    /// Provides the start state
    /// </summary>
    /// <returns>state</returns>
    public State<StateEnum> getStartState()
    {
        if(m_startState == null)
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
        if(m_startState == null)
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
    public List<Transition<StateEnum>> getAllTransitionsFrom(State<StateEnum> ai_state)
    {
        List<Transition<StateEnum>> w_result = new List<Transition<StateEnum>>();

        // browse all transitions
        foreach(var transition in m_transitions)
        {
            // if the transitions starts from the desired state add it to result list
            if(transition.From.IsSameState(ai_state))
            {
                w_result.Add(transition);
            }
        }

        return w_result;
    }
    #endregion
}
