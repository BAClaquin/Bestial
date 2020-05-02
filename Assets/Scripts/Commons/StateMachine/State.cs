using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

// Operation that can be called during a state
public delegate bool StateOperationDelegate(IGame ai_game);

public class State<StateEnum> where StateEnum : System.Enum
{
    #region Members
    public StateEnum ID { get; private set; }
    #endregion

    #region Private Members
    /// <summary>
    /// Will be called when entering the state
    /// </summary>
    StateOperationDelegate m_onEnterDelegate;
    /// <summary>
    /// Will be called during the state
    /// </summary>
    StateOperationDelegate m_onStateDelegate;
    /// <summary>
    /// Will be called when leaving the state
    /// </summary>
    StateOperationDelegate m_onLeaveDelegate;
    /// <summary>
    /// Internal state machine provides functions regarding state maching status
    /// </summary>
    IInternalStateMachine<StateEnum> m_internalStateMachine;
    /// <summary>
    /// Provides game status
    /// </summary>
    IGame m_game;
    #endregion

    #region Constructors
    /// <summary>
    /// Constructor for Abstract State
    /// </summary>
    /// <param name="ai_id">ID of the state</param>
    public State(StateEnum ai_id, IInternalStateMachine<StateEnum> ai_stateMachine, IGame ai_game)
    {
        ID = ai_id;
        m_internalStateMachine = ai_stateMachine;
        m_game = ai_game;
    }
    #endregion

    #region Public 
    /// <summary>
    /// Tells if the current state has the same ID as the other
    /// </summary>
    /// <param name="ai_other">other state to compare to</param>
    /// <returns>True if same ID, false otherwise</returns>
    public bool IsSameState(State<StateEnum> ai_other)
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
    ///  Add a function that will be called on enter of the state
    /// </summary>
    /// <param name="ai_delegate">function to call</param>
    public void SetOnEnterFuntion(StateOperationDelegate ai_delegate)
    {
        if (m_onEnterDelegate != null)
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "On enter delegate function has already been set. It will be replaced by this new one.");
        }
        m_onEnterDelegate = ai_delegate;
    }

    /// <summary>
    /// Is called when you enter the state for the first time
    /// </summary>
    public void OnEnter()
    {
        Tracer.Instance.Trace(TraceLevel.INFO2, "Entering state" + ToString());
        // if a function has been set, call it
        m_onEnterDelegate?.Invoke(m_game);
    }

    /// <summary>
    ///  Adds a function that will be called on enter of the state
    /// </summary>
    /// <param name="ai_delegate">function to call</param>
    public void SetOnStateFunction(StateOperationDelegate ai_delegate)
    {
        if (m_onStateDelegate != null)
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "On state delegate function has already been set. It will be replaced by this new one.");
        }
        m_onStateDelegate = ai_delegate;
    }

    /// <summary>
    /// Is called when you are on the state
    /// </summary>
    public void OnState()
    {
        Tracer.Instance.Trace(TraceLevel.DEBUG, "On state" + ToString());
        // if a function has been set, call it
        m_onStateDelegate?.Invoke(m_game);
    }

    /// <summary>
    ///  Adds a function that will be called on enter of the state
    /// </summary>
    /// <param name="ai_delegate">function to call</param>
    public void SetOnLeaveFunction(StateOperationDelegate ai_delegate)
    {
        if (m_onLeaveDelegate != null)
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "On leave delegate function has already been set. It will be replaced by this new one.");
        }
        m_onLeaveDelegate = ai_delegate;
    }

    /// <summary>
    /// Is called when you leave the state
    /// </summary>
    public void OnLeave()
    {
        Tracer.Instance.Trace(TraceLevel.INFO2, "Leaving state " + ToString());
        // if a function has been set, call it
        m_onLeaveDelegate?.Invoke(m_game);
    }
    #endregion
}
