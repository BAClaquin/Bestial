using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Global Game Manager where you can manage Games
/// </summary>
public class Game : MonoBehaviour
{
    #region Public Unity Members
    [Header(UnityHeaders.Gameplay)]
    /// <summary>
    /// Current Map Played
    /// </summary>
    public Map CurrentMap;

    [Header(UnityHeaders.Developp)]
    /// <summary>
    /// Tracer module for this game
    /// </summary>
    public GameObject TracerModule;
    public Text CurrentPlayerPlaceholder;
    public Player[] Players;

    #endregion

    #region Private Memebrs
    Unit m_selectedUnit;
    #endregion

    private int CurrentPlayerTurn = -1;
    private bool GameIsOver = false;

    #region UI Functions
    void Start()
    {
        if (TracerModule == null)
        {
            print("ERROR : No trace module provided.");
            throw new System.NullReferenceException("TracerModule is null");
        }
        
        // check for UnityConstruction values
        if (CurrentMap == null)
        {
            Tracer.Instance.Trace(TraceLevel.ERROR, "No map provided for object Game !");
            throw new System.NullReferenceException("CurrentMap is null");
        }

        NextTurn();
        Tracer.Instance.Trace(TraceLevel.INFO, "Game started !");

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Function to call when going to the next turn
    /// </summary>
    public void NextTurn()
    {
        resetCurrentAction();
        if (GameIsOver)
        {
            throw new NotImplementedException("Game Over screen not implemented");
        }else
        {
            SetNextPlayer();
            CurrentMap.ResetUnits();
        }
    }

    private void SetNextPlayer()
    {
        CurrentPlayerTurn = (CurrentPlayerTurn + 1) % Players.Length;
        CurrentPlayerPlaceholder.text = Players[CurrentPlayerTurn].Name;
        CurrentPlayerPlaceholder.color = Players[CurrentPlayerTurn].UnitColor;
    }

    /// <summary>
    /// Occurs when an unit is selected
    /// </summary>
    /// <param name="ai_unit"></param>
    public void onSelectedUnit(Unit ai_unit)
    {
        // reset current action if selecting another unit
        resetCurrentAction();

        // store selected unit
        m_selectedUnit = ai_unit;
        // if unit hasn't played
        if(!m_selectedUnit.hasMoved() && unitBelongToCurrentPlayer(m_selectedUnit))
        {
            ai_unit.Highlight();
            CurrentMap.DisplayAvailableMoves(m_selectedUnit);
        }
    }

    /// <summary>
    /// Occurs when a tile is selected
    /// </summary>
    /// <param name="ai_tile"></param>
    public void onSelectedTile(Tile ai_tile)
    {
        if(m_selectedUnit != null)
        {
            // DUMB IMPLEM
            if (ai_tile.isTaggedAccessible() && !m_selectedUnit.hasMoved() && unitBelongToCurrentPlayer(m_selectedUnit))
            {
                // move unit
                m_selectedUnit.moveTo(ai_tile.getGridPosition());
                // disable next moves
                m_selectedUnit.Disable();
            }
            // reset actions
            resetCurrentAction();
        }
    }

    private bool unitBelongToCurrentPlayer(Unit m_selectedUnit)
    {
        return CurrentPlayer().HasUnit(m_selectedUnit);
    }

    private Player CurrentPlayer()
    {
        return Players[CurrentPlayerTurn];
    }

    #endregion

    #region Private Functions
    /// <summary>
    /// Resets the current action
    /// </summary>
    void resetCurrentAction()
    {
        if(m_selectedUnit != null)
        {
            // reset availability of an nit
            m_selectedUnit.Unselect();
            // reset highlighted tiles
            CurrentMap.ResetAvailableMoves();
            // no unit is being selected
            m_selectedUnit = null;
        }
        // else no actions to reset for the moment
    }
    #endregion

}
