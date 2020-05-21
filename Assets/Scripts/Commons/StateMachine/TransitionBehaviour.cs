namespace StateMachine
{
    public abstract class TransitionBehaviour<TStateEnum, TStateMachineWorker, TEventSystem>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker
    {
        protected IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> m_internalStateMachine;

        public TransitionBehaviour(IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventSystem> ai_internalStateMachine)
        {
            m_internalStateMachine = ai_internalStateMachine;
        }

       /// <summary>
       /// Evaluate if the transition can occur or not
       /// </summary>
        public abstract bool EvaluateCondition();


        /// <summary>
        /// Executes the transition  :
        ///  calls : ConsumeAndStore
        ///  calls : OnTransitionAction
        /// </summary>
        public void ExecuteTransition()
        {
            ConsumeAndStore();
            OnTransitionAction();
        }

        /// <summary>
        /// If transition is based on an event : must implement a consume and store data of the event
        /// </summary>
        public virtual void ConsumeAndStore() { }

        /// <summary>
        /// If some transition actions must occur, implement it
        /// </summary>
        public virtual void OnTransitionAction() { }
    }
}