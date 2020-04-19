using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion

    #region Private Memebrs
    Unit m_selectedUnit;
    #endregion

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
        // reset all units (dumb implem for the momeent)
        CurrentMap.ResetUnits();
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
        if(!m_selectedUnit.hasMoved())
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
            if (ai_tile.isTaggedAccessible() && !m_selectedUnit.hasMoved())
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
