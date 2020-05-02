using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool EvaluateConditionDelegate<StateEnum>(IInternalStateMachine<StateEnum> ai_stateMachine, IGame ai_game) where StateEnum : System.Enum;

public delegate void OnTransitionDelegate(IGame ai_game);

public class Transition<StateEnum>
    where StateEnum : System.Enum
{
    #region Condition for transition
    /// <summary>
    /// Externally provided condition to test if transition is possible
    /// </summary>
    EvaluateConditionDelegate<StateEnum> m_conditionDelegate;
    #endregion

    #region Members
    /// <summary>
    /// State to transit from
    /// </summary>
    public State<StateEnum> From { get; private set; }
    /// <summary>
    /// State to transit to
    /// </summary>
    public State<StateEnum> To { get; private set; }
    #endregion

    #region Private Members
    IInternalStateMachine<StateEnum> m_internalStateMachine;
    IGame m_game;
    OnTransitionDelegate m_onTransitionDelegate;
    #endregion

    #region Constructors
    public Transition(State<StateEnum> ai_from, State<StateEnum> ai_to,
        EvaluateConditionDelegate<StateEnum> ai_conditionFunction,
        IInternalStateMachine<StateEnum> ai_stateMachine, IGame ai_game)
    {
        From = ai_from;
        To = ai_to;
        m_conditionDelegate = ai_conditionFunction;
        m_internalStateMachine = ai_stateMachine;
        m_game = ai_game;
    }
    #endregion

    #region Functions
    /// <summary>
    /// Tells is this transition is the same (FROM and TO) than the other provided one
    /// </summary>
    /// <param name="ai_other">other transition you wanna test equality for</param>
    /// <returns>True if FROM and TO are the same, false otherwise</returns>
    public bool isSameTransition(Transition<StateEnum> ai_other)
    {
        return From.IsSameState(ai_other.From) && To.IsSameState(ai_other.To);
    }

    /// <summary>
    /// Describes transition as a string
    /// </summary>
    /// <returns>Result of description</returns>
    public override string ToString()
    {
        return "{Transition from " + From.ToString() + " to " + To.ToString() +"}";
    }

    /// <summary>
    /// Evaluates if the transition is possible
    /// </summary>
    /// <returns>True if transition possible, false otherwise</returns>
    public bool evaluateCondition()
    {
        // check if current state matching start state
        if( ! m_internalStateMachine.getCurrentState().IsSameState(From) )
        {
            // should not happen if stateMachine correctly implemented
            Tracer.Instance.Trace(TraceLevel.ERROR, "Evaluating transition from state " + m_internalStateMachine.getCurrentState().ToString()
                   + "but this transition starts from state " + From.ToString());
            return false;
        }

        // preconditions ok, test if transition is allowed
        return m_conditionDelegate(m_internalStateMachine, m_game);
    }


    /// <summary>
    ///  Adds a function that will be called when transiting
    /// </summary>
    /// <param name="ai_delegate">function to call</param>
    public void SetTransitionAction(OnTransitionDelegate ai_delegate)
    {
        if (m_onTransitionDelegate != null)
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "On transition delegate function has already been set. It will be replaced by this new one.");
        }

        m_onTransitionDelegate = ai_delegate;
    }

    /// <summary>
    /// If a tranistion action is set : calls it
    /// </summary>
    public void CallTransitionAction()
    {
        // if a trasition action is defined call it
        m_onTransitionDelegate?.Invoke(m_game);
    }
    #endregion
}
