using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<StateEnum, Worker, EventSystem>;
    public class Attack  : StateImpl
    {
        public Attack() : base(StateEnum.ATTACK) { }

        public static bool toUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {
            return true;
        }

        override
        public void OnEnter()
        {
            Utils.attackUnit(m_internalStateMachine);
        }
    }
}
