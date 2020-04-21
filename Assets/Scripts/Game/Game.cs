using System;
using System.Collections;
using System.Collections.Generic;
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
    bool m_actionAfterMoveMode = false;
    List<Unit> m_targetableUnits;
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
        if (GameIsOver)
        {
            throw new NotImplementedException("Game Over screen not implemented");
        }
        else
        {
            resetCurrentAction();
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

        //bool doubleClick = m_selectedUnit != null && m_selectedUnit == ai_unit && unitBelongToCurrentPlayer(ai_unit);
        //bool clickOnTargetableUnit = m_selectedUnit != null  && m_targetableUnits.Contains(ai_unit);

        if (m_selectedUnit != null && m_targetableUnits != null && m_targetableUnits.Contains(ai_unit) && !unitBelongToCurrentPlayer(ai_unit))
        {
            attackEnnemi(ai_unit);
        }
        else if (m_selectedUnit != null)
        {
            if (m_actionAfterMoveMode)
            {
                restUnit();
                selectUnit(ai_unit);
            }
            else
            {
                if (m_selectedUnit == ai_unit)
                {
                    restUnit();
                }
                else
                {
                    selectUnit(ai_unit);
                }
            }
        }
        else
        {
            selectUnit(ai_unit);
        }



        /*
        if (doubleClick) // clicked twice on the same unit (and unit belongs to current player) => restUnit
        {
            restUnit();
        }
        else if (clickOnTargetableUnit) 
        {
            attackEnnemi(ai_unit);
        }
        else // select ai_unit
        {
            selectUnit(ai_unit);
        }*/
    }

    private void selectUnit(Unit ai_unit)
    {
        print("select ai_unit");
        resetCurrentAction();
        // if unit hasn't played
        if (!ai_unit.hasMoved() && unitBelongToCurrentPlayer(ai_unit))
        {
            m_selectedUnit = ai_unit;
            ai_unit.Highlight();
            CurrentMap.DisplayAvailableMoves(ai_unit);
            m_targetableUnits = CurrentMap.DisplayAvailableTargets(CurrentPlayer(), ai_unit);
        }
    }

    private void attackEnnemi(Unit ai_unit)
    {
        CurrentMap.removeUnit(ai_unit);
        ai_unit.Kill(); // kill targeted unit
        m_selectedUnit.Disable();
        resetCurrentAction();
    }

    /// <summary>
    /// Occurs when a tile is selected
    /// </summary>
    /// <param name="ai_tile"></param>
    public void onSelectedTile(Tile ai_tile)
    {
        if (m_selectedUnit != null)
        {
            print("unit slected");
            if (m_actionAfterMoveMode) // unit already selected + attackAfterMove + click on some tile => unit rests
            {
                restUnit();
            }
            else // unit selected + unit not moved + click on some tiles => unit moves
            {
                bool unitCanMoveToTile = ai_tile.isTaggedAccessible() && !m_selectedUnit.hasMoved() && unitBelongToCurrentPlayer(m_selectedUnit);
                if (unitCanMoveToTile)
                {
                    StartCoroutine(moveUnit(ai_tile));
                }
            }
        }
    }

    private void restUnit()
    {
        m_actionAfterMoveMode = false;
        m_selectedUnit.Disable();
        resetCurrentAction();
    }

    private IEnumerator moveUnit(Tile ai_tile)
    {
        CurrentMap.ResetAvailableMoves();
        yield return m_selectedUnit.moveTo(ai_tile.getGridPosition());

        // setup actions such as attacking or resting
        actionsAfterMove(ai_tile);
    }

    private void actionsAfterMove(Tile ai_tile)
    {
        m_targetableUnits = CurrentMap.DisplayAvailableTargets(CurrentPlayer(), m_selectedUnit);
        bool w_thereAreTargetableEnnemyUnits = m_targetableUnits.Count != 0;
        if (w_thereAreTargetableEnnemyUnits) // if unit can attack, user must chose betweek attack or rest
        {
            m_actionAfterMoveMode = true;
        }
        else // otherwise the unit rests
        {
            m_selectedUnit.Disable();
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
        m_actionAfterMoveMode = false;
        if (m_selectedUnit != null)
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
