using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// DRAFT EN COURS
/*
public enum GameState
{
    WAIT_GAME_ACTION,
    WAIT_UNIT_ACTION,
    UNIT_MOVED,
    UNIT_ATTACKED,
    NEXT_TURN
}

public interface GameActions
{
    void DisplayPossibleMoves();
    void Move();
    void CancelCurrentAction();
}

public interface GameEvent
{
    void selectUnit(Unit ai_unit);
    void selectTile(Tile ai_tile);
}

/// <summary>
/// State chart ruling actions in the game
/// </summary>
public class GameStateChart : GameEvent
{
    #region Private Members
    GameState m_current_state;
    Unit m_selectedUnit;
    #endregion

    #region Constructors
    /// <summary>
    /// Default constructor for game state
    /// </summary>
    public GameStateChart()
    {
        m_current_state = GameState.WAIT_GAME_ACTION;
    }
    #endregion

    #region Game Events
    void selectTile(Tile ai_tile)
    {
        if(m_current_state == GameState.WAIT_UNIT_ACTION)
        {

        }
        else
        {
            //Tracer.Instance.Trace("DEBUG : no action on selected tile for state " + m_current_state.ToString());
        }
    }

    void selectUnit(Unit ai_unit)
    {
        // Waiting any game action
        if (m_current_state == GameState.WAIT_GAME_ACTION)
        {
            DisplayPossibleUnitActions(ai_unit);
            goToState(GameState.WAIT_UNIT_ACTION);
        }
        else if(m_current_state == GameState.WAIT_UNIT_ACTION)
        {
            if(m_selectedUnit != ai_unit) // and unit is in same team @TODO
            {
                goToState(GameState.WAIT_GAME_ACTION);
                selectUnit(ai_unit);
            }
        }
    }
    #endregion

    #region GameActions
    /// <summary>
    /// Displays all possible actions for this unit
    /// </summary>
    private void DisplayPossibleUnitActions(Unit ai_unit)
    {
        DisplayPossibleMoves(ai_unit);
        //DisplayPossibleAttacks(ai_unit);
    }

    private void DisplayPossibleMoves(Unit ai_unit)
    {

    }

    private void DisplayPossibleAttacks(Unit ai_unit)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region StateChartFunctions
    void goToState(GameState ai_state)
    {
        bool w_requestAccepted = true;
        Tracer.Instance.Trace(TraceLevel.INFO, "Requested transition from " + m_current_state.ToString() + " to " + ai_state.ToString());

        if (m_current_state == GameState.WAIT_UNIT_ACTION && ai_state == GameState.WAIT_GAME_ACTION)
        {
            transit_from_WaitUnitAction_to_waitGameAction();
        }
        else
        {
            w_requestAccepted = false;
            Tracer.Instance.Trace(TraceLevel.INFO, "Requested transition from " + m_current_state.ToString() + " to " + ai_state.ToString());
        }
        if(w_requestAccepted)
        {
            m_current_state = ai_state;
        }
        Tracer.Instance.Trace(TraceLevel.INFO, "Game state is " + m_current_state.ToString());
    }


    // TRANSITIONS
    public void transit_from_WaitUnitAction_to_waitGameAction()
    {
        // reset unit posible actions
    }
    #endregion
}
*/