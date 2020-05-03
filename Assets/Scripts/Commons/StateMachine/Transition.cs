using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool EvaluateConditionDelegate<TStateMachineWorker>(TStateMachineWorker ai_worker, IGame ai_game)
    where TStateMachineWorker : IStateMachineWorker;

public delegate void OnTransitionDelegate<TStateMachineWorker>(TStateMachineWorker ai_worker, IGame ai_game) where TStateMachineWorker : IStateMachineWorker;

public class Transition<TStateEnum, TStateMachineWorker>
    where TStateEnum : System.Enum
    where TStateMachineWorker : IStateMachineWorker
{
    #region Condition for transition
    /// <summary>
    /// Externally provided condition to test if transition is possible
    /// </summary>
    EvaluateConditionDelegate<TStateMachineWorker> m_conditionDelegate;
    #endregion

    #region Members
    /// <summary>
    /// State to transit from
    /// </summary>
    public TStateEnum From { get; private set; }
    /// <summary>
    /// State to transit to
    /// </summary>
    public TStateEnum To { get; private set; }
    #endregion

    #region Private Members
    IInternalStateMachine<TStateEnum, TStateMachineWorker> m_internalStateMachine;
    IGame m_game;
    TStateMachineWorker m_worker;
    OnTransitionDelegate<TStateMachineWorker> m_onTransitionDelegate;
    #endregion

    #region Constructors
    public Transition(TStateEnum ai_from, TStateEnum ai_to,
        EvaluateConditionDelegate<TStateMachineWorker> ai_conditionFunction,
        IInternalStateMachine<TStateEnum, TStateMachineWorker> ai_stateMachine, IGame ai_game, TStateMachineWorker ai_worker)
    {
        From = ai_from;
        To = ai_to;
        m_conditionDelegate = ai_conditionFunction;
        m_internalStateMachine = ai_stateMachine;
        m_game = ai_game;
        m_worker = ai_worker;
    }
    #endregion

    #region Functions
    /// <summary>
    /// Tells is this transition is the same (FROM and TO) than the other provided one
    /// </summary>
    /// <param name="ai_other">other transition you wanna test equality for</param>
    /// <returns>True if FROM and TO are the same, false otherwise</returns>
    public bool isSameTransition(Transition<TStateEnum, TStateMachineWorker> ai_other)
    {
        return From.Equals(ai_other.From) && To.Equals(ai_other.To);
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
        if (!m_internalStateMachine.getCurrentState().ID.Equals(From) )
        {
            // should not happen if stateMachine correctly implemented
            Tracer.Instance.Trace(TraceLevel.ERROR, "Evaluating transition from state " + m_internalStateMachine.getCurrentState().ToString()
                   + "but this transition starts from state " + From.ToString());
            return false;
        }

        // preconditions ok, test if transition is allowed
        return m_conditionDelegate(m_worker, m_game);
    }


    /// <summary>
    ///  Adds a function that will be called when transiting
    /// </summary>
    /// <param name="ai_delegate">function to call</param>
    public void SetTransitionAction(OnTransitionDelegate<TStateMachineWorker> ai_delegate)
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
        m_onTransitionDelegate?.Invoke(m_worker, m_game);
    }
    #endregion
}
