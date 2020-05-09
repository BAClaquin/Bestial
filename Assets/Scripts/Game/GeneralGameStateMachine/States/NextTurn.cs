using StateMachine;

namespace GameStateMachine
{
    internal class NextTurn : StateImpl
    {
        public NextTurn() : base(StateEnum.NEXT_TURN) { }
    }
}