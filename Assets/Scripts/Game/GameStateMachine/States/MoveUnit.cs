using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameStateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;
    public class MoveUnit : GameStateMachine.GameState
    {
        public MoveUnit() : base(GameStates.MOVE_UNIT) { }

        override protected void OnEnterImpl()
        {
            m_internalStateMachine.GetGame().MoveUnitToTile(m_internalStateMachine.GetWorker().CurrentUnit, m_internalStateMachine.GetWorker().LastSelectedTile);
        }

    }
}
