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
    /// <summary>
    /// Is unit playable by current player
    /// Has remainning possible actions
    /// Is not disabled
    /// </summary>
    /// <param name="ai_unit"></param>
    /// <returns></returns>
    bool UnitIsPlayable(Unit ai_unit);
    bool UnitCanMoveToTile(Unit ai_unit, Tile ai_tile);
    bool UnitIsAttackableByUnit(Unit ai_unit, Unit ai_target);
    void HighlightAccessibleTiles(Unit ai_unit);
    void DisplayAvailableTargets(Unit ai_unit);
    void UnlightAllActions();
    void moveUnitToTile(Unit ai_unit, Tile ai_tile);
    void AttackEnnemi(Unit ai_unit, Unit ai_target);
    /// <summary>
    /// Tells if unit belongs to current player
    /// </summary>
    /// <param name="unit">Unit</param>
    /// <returns>True if belongs, false otherwise</returns>
    bool UnitBelongsToCurrentPlayer(Unit unit);
    /// <summary>
    /// Function to call when going to the next turn
    /// </summary>
    void ExecuteNextTurn();
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

    #region Private Members
    Unit m_selectedUnit;
    List<Unit> m_targetableUnits;
    #endregion

    private int CurrentPlayerTurn = -1;
    private bool GameIsOver = false;
    private IStateMachine<IGameEventEmitter> m_stateMachine;

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

        ExecuteNextTurn();
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
    /// Requests a next turn that will be executed when possible
    /// </summary>
    public void RequestNextTurn()
    {
        m_stateMachine.GetEventEmiter().GetNextTurnEmitter().Raise();
    }

    /// <summary>
    /// Function to call when going to the next turn
    /// </summary>
    public void ExecuteNextTurn()
    {
        if (GameIsOver)
        {
            throw new NotImplementedException("Game Over screen not implemented");
        }
        else
        {
            SetNextPlayer();
            CurrentMap.ResetAvailableMoves();
            CurrentMap.ResetUnits();
        }
    }

    public bool UnitIsPlayable(Unit ai_unit)
    {
        // dumb implem to refactor with msart action management system
        if ( ai_unit.IsDisabled() || !UnitBelongsToCurrentPlayer(ai_unit) || ai_unit.HasConsumedAllAttacks())
        {
            return false;
        }
        if(ai_unit.CanAttack() && CurrentMap.HasAvailableTargets(CurrentPlayer(), ai_unit))
        {
            return true;
        }
        return ai_unit.CanMove();
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
        m_stateMachine.GetEventEmiter().GetUnitSelectedEmitter().Raise(ai_unit);
    }

    public void HighlightAccessibleTiles(Unit ai_unit)
    {
        if (UnitIsPlayable(ai_unit))
        {
            CurrentMap.DisplayAvailableMoves(ai_unit);
        }
    }

    public void DisplayAvailableTargets(Unit ai_unit)
    {
        m_targetableUnits = CurrentMap.DisplayAvailableTargets(CurrentPlayer(), ai_unit);
    }


    public void UnlightAllActions()
    {
        CurrentMap.ResetAvailableMoves();
    }


    /// <summary>
    /// Occurs when a tile is selected
    /// </summary>
    /// <param name="ai_tile"></param>
    public void onSelectedTile(Tile ai_tile)
    {
        m_stateMachine.GetEventEmiter().GetTileSelectedEmitter().Raise(ai_tile);
    }


    public void moveUnitToTile(Unit m_selectedUnit, Tile ai_tile)
    {        
        StartCoroutine(moveTo(m_selectedUnit, ai_tile));
       
    }

    private IEnumerator moveTo(Unit m_selectedUnit, Tile ai_tile)
    {
        yield return m_selectedUnit.moveTo(CurrentMap.getComputedPathTo(ai_tile));
        m_stateMachine.GetEventEmiter().GetMoveIsOverEmitter().Raise();
    }

    public bool UnitBelongsToCurrentPlayer(Unit m_selectedUnit)
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
        if (m_selectedUnit != null)
        {
            // reset highlighted tiles
            CurrentMap.ResetAvailableMoves();
            // no unit is being selected
            m_selectedUnit = null;
        }
        // else no actions to reset for the moment
    }


    public bool UnitCanMoveToTile(Unit ai_unit, Tile ai_tile)
    {
        return ai_tile.isTaggedAccessible() && ai_unit.CanMove() && UnitBelongsToCurrentPlayer(ai_unit);
    }

    public bool UnitIsAttackableByUnit(Unit ai_unit, Unit ai_target)
    {
        return m_targetableUnits.Contains(ai_target);
    }

    public void AttackEnnemi(Unit ai_unit, Unit ai_targetUnit)
    {
        // TODO: actually use ai_unit
        CurrentMap.removeUnit(ai_targetUnit);
        ai_targetUnit.Kill(); // kill targeted unit
        ai_unit.ConsumeAttack(); // unit has consumed its attack
    }
    #endregion

}
