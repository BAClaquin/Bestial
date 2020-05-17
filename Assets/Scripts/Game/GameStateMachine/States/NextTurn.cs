using StateMachine;

namespace GameStateMachine
{
    internal class NextTurn : GameState
    {
        public NextTurn() : base(GameStates.NEXT_TURN) { }
    }
}