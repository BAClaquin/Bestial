using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameStateMachine
{

    using IInternalStateMachine = IInternalStateMachine<StateEnum, Worker, EventSystem>;

    public class WaitUnitActionUnitSelected : StateImpl
    {

        public WaitUnitActionUnitSelected() : base(StateEnum.WAIT_UNIT_ACTION_UNIT_SELECTED) { }

        internal static bool toAttack(IInternalStateMachine ai_internalStateMachine)
        {                        
            // unit is attackable => attack
            return Utils.unitIsAttackable(ai_internalStateMachine);
        }

        internal static bool toUnitSelected(IInternalStateMachine ai_internalStateMachine)
        {
            //unit is not attackable => select unit
            if (!Utils.unitIsAttackable(ai_internalStateMachine))
            {
                ai_internalStateMachine.GetWorker().m_currentUnit = ai_internalStateMachine.GetWorker().m_targetUnit;
                return true;
            }

            return false;
        }

        override
        public void OnLeave()
        {
            Utils.undisplayPossibleAction(m_internalStateMachine);
        }

    }
}
