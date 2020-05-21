using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;
    public class Attack  : GameState
    {
        public Attack() : base(GameStates.ATTACK) { }

        override
        protected void OnEnterImpl()
        {
            Utils.attackUnit(m_internalStateMachine);
        }
    }
}
