using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System;

namespace GameStateMachine
{
    public class GameWorker : StateMachine.IStateMachineWorker
    {

        public Unit CurrentUnit { get; private set;}
        public Unit LastSelectedUnit { get; set; }
        public Tile LastSelectedTile { get; set; }


        /// <summary>
        /// Sets the current unit manipulated by player
        /// and highlights it
        /// </summary>
        /// <param name="ai_unit">Unit to set as current one</param>
        public void SelectCurrentUnit(Unit ai_unit)
        {
            CurrentUnit = ai_unit;
            CurrentUnit.SetSelected(true);
        }

        public void DeselectCurrentUnit()
        {
            CurrentUnit.SetSelected(false);
        }

        public void HighlightCurrentUnit(bool ai_highlight)
        {
            CurrentUnit.Highlight(ai_highlight);
        }

        public void reset()
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "Implement the reset");
        }

    }
}

