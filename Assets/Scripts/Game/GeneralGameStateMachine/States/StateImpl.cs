using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditorInternal;

namespace GameStateMachine
{
    public abstract class StateImpl : State<StateEnum, Worker, EventSystem>
    {
        public StateImpl(StateEnum ai_id) : base(ai_id)
        {
        }

        public override void OnEnter()
        {
        }

        public override void OnLeave()
        {
        }

        public override void OnState()
        {
        }

    }
}
