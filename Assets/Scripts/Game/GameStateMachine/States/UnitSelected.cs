using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<GameStates, GameWorker, GameEventSytstem>;

    public class UnitSelected : GameState
    {
        public UnitSelected(): base(GameStates.UNIT_SELECTED) {}

        /// <summary>
        /// Describing unit when entering this state
        /// </summary>
        protected override void OnEnterImpl()
        {
            // Display Unit Info
            Tracer.Instance.Trace(TraceLevel.INFO1, "Selected Unit = " + m_internalStateMachine.GetWorker().LastSelectedUnit.Describe());
        }
    }
}
