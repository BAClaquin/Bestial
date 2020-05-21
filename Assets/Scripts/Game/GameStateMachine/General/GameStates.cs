using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{
    public enum GameStates
    {
        WAIT_GAME_ACTION,
        WAIT_UNIT_ACTION,
        UNIT_SELECTED,       
        MOVE_UNIT,
        ATTACK,
        NEXT_TURN
    }
}
