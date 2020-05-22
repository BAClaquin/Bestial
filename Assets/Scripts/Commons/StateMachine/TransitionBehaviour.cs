namespace StateMachine
{
    public abstract class TransitionBehaviour<TStateEnum, TStateMachineWorker, TEventConsumer>
        where TStateEnum : System.Enum
        where TStateMachineWorker : IStateMachineWorker
    {
        protected TEventConsumer m_eventConsumer;
        protected TStateMachineWorker m_worker;
        protected IGame m_game;

        public TransitionBehaviour(IInternalStateMachine<TStateEnum, TStateMachineWorker, TEventConsumer> ai_internalStateMachine)
        {
            m_eventConsumer = ai_internalStateMachine.GetEventConsumer();
            m_worker = ai_internalStateMachine.GetWorker();
            m_game = ai_internalStateMachine.GetGame();
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