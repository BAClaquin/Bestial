using StateMachine;

namespace GameStateMachine
{
    internal class NextTurn : GameState
    {
        public NextTurn() : base(GameStates.NEXT_TURN) { }

        /// <summary>
        /// Execute next turn
        /// </summary>
        protected override void OnEnterImpl()
        {
            m_internalStateMachine.GetGame().ExecuteNextTurn();
        }
    }
}