using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Template instanciation for GameStateChart

namespace GameStateMachine
{
    /// <summary>
    /// To ease declaration of Game State
    /// </summary>
    public class GameState : StateMachine.State<GameStates, GameWorker, GameEventSytstem>
    {
        public GameState(GameStates ai_id) : base(ai_id) { }
    }
}