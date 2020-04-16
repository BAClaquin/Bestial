using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global Game Manager where you can manage Games
/// </summary>
public class Game : MonoBehaviour
{
    #region Public Members
    /// <summary>
    /// Current Map Played
    /// </summary>
    public Map CurrentMap;
    #endregion

    #region Private Memebrs
    Unit m_selectedUnit;
    #endregion

    #region UI Functions
    void Start()
    {
        // check for UnityConstruction values
        if (CurrentMap == null)
         {
             print("ERROR : No map provided for object Game");
             throw new System.NullReferenceException("CurrentMap is null");
         }
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Occurs when an unit is selected
    /// </summary>
    /// <param name="ai_unit"></param>
    public void onSelectedUnit(Unit ai_unit)
    {
        // store selected unit
        m_selectedUnit = ai_unit;
        // if unit hasn't played
        if(!m_selectedUnit.hasMoved())
        {
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
            if(ai_tile.isTaggedAccessible() && !m_selectedUnit.hasMoved())
            {
                m_selectedUnit.moveTo(ai_tile.getGridPosition());
            }
        }
    }
    #endregion
}
