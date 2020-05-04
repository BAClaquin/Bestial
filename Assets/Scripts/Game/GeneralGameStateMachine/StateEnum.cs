using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{
    public enum GameStateEnum
    {
        WAIT_GAME_ACTION,
        WAIT_UNIT_ACTION_IDLE,
        WAIT_UNIT_ACTION_TILE_SELECTED,
        WAIT_UNIT_ACTION_UNIT_SELECTED,
        UNIT_SELECTED,       
        MOVE_UNIT,
        ATTACK,
        NEXT_TURN
    }
}
