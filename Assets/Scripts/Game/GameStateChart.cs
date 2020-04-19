using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    WAIT_ACTION,
    WAIT_MOVE,
    WAIT_ATTACK_CHOICE
}

public interface GameActions
{
    void DisplayPossibleMoves();
    void Move();
    void CancelCurrentAction();
}

public interface GameEvent
{
    void selectUnit();
    void selectTile(Tile ai_tile);
}

/// <summary>
/// State chart ruling actions in the game
/// </summary>
public class GameStateChart : GameEvent
{
    #region Private Members
    GameState m_current_state;
    #endregion

    #region Constructors
    /// <summary>
    /// Default constructor for game state
    /// </summary>
    public GameStateChart()
    {
        m_current_state = GameState.WAIT_ACTION;
    }
    #endregion

    #region Game Events
    void GameEvent.selectTile(Tile ai_tile)
    {
        if(m_current_state == GameState.WAIT_MOVE)
        {

        }
        else
        {
            //Tracer.Instance.Trace("DEBUG : no action on selected tile for state " + m_current_state.ToString());
        }
    }

    void GameEvent.selectUnit()
    {
        throw new System.NotImplementedException();
    }
    #endregion


}