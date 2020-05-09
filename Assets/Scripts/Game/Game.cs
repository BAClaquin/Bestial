using GameStateMachine;
using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Interfaces provided for game information and action request
/// </summary>
public interface IGame
{
    bool unitIsPlayable(Unit ai_unit);
    bool tileIsInReachOfUnit(Unit ai_unit, Tile ai_tile);
    bool unitIsAttackableByUnit(Unit ai_unit, Unit ai_target);
    void highlightAccessibleTiles(Unit ai_unit);
    void unHighlightAccessibleTiles();
    void moveUnitToTile(Unit ai_unit, Tile ai_tile);
    void attackEnnemi(Unit ai_unit, Unit ai_target);
}

/// <summary>
/// Global Game Manager where you can manage Games
/// </summary>
public class Game : MonoBehaviour,IGame
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
    private IStateMachine<EventSystem> m_stateMachine;

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

        Factory factory = new Factory(this);
        m_stateMachine = factory.Create("my state machine");
        m_stateMachine.Start();

        NextTurn();
        Tracer.Instance.Trace(TraceLevel.INFO1, "Game started !");

    }

    // Update is called once per frame
    void Update()
    {
        if(m_stateMachine != null)
        {
            m_stateMachine.ComputeState();
        }
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

    public bool unitIsPlayable(Unit ai_unit)
    {
        return !ai_unit.hasMoved() && unitBelongToCurrentPlayer(ai_unit);
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
        m_stateMachine.GetEventSystem().RaiseUnitSelectedEvent(ai_unit);       
    }

    public void highlightAccessibleTiles(Unit ai_unit)
    {
        if (unitIsPlayable(ai_unit))
        {
            CurrentMap.DisplayAvailableMoves(ai_unit);
            m_targetableUnits = CurrentMap.DisplayAvailableTargets(CurrentPlayer(), ai_unit);
        }
    }

    public void unHighlightAccessibleTiles()
    {
        CurrentMap.ResetAvailableMoves();
    }


    /// <summary>
    /// Occurs when a tile is selected
    /// </summary>
    /// <param name="ai_tile"></param>
    public void onSelectedTile(Tile ai_tile)
    {
        m_stateMachine.GetEventSystem().RaiseTileSelctedEvent(ai_tile);
    }


    public void moveUnitToTile(Unit m_selectedUnit, Tile ai_tile)
    {        
        StartCoroutine(moveTo(m_selectedUnit, ai_tile));
       
    }

    private IEnumerator moveTo(Unit m_selectedUnit, Tile ai_tile)
    {
        yield return m_selectedUnit.moveTo(CurrentMap.getComputedPathTo(ai_tile));
        m_stateMachine.GetEventSystem().RaiseAnimationIsOverEvent();
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

    public bool tileIsInReachOfUnit(Unit ai_unit, Tile ai_tile)
    {
        return ai_tile.isTaggedAccessible() && !ai_unit.hasMoved() && unitBelongToCurrentPlayer(ai_unit);
    }

    public bool unitIsAttackableByUnit(Unit ai_unit, Unit ai_target)
    {
        //List<Unit> w_targetableUnits = CurrentMap.DisplayAvailableTargets(CurrentPlayer(), ai_unit);
        return m_targetableUnits.Contains(ai_unit);
    }

    public void attackEnnemi(Unit ai_unit, Unit ai_targetUnit)
    {
        // TODO: actually use ai_unit
        CurrentMap.removeUnit(ai_targetUnit);
        ai_targetUnit.Kill(); // kill targeted unit
        //ai_unit.Disable();
    }



    #endregion

}
