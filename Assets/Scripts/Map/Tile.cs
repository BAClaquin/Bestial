using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

/// <summary>
/// Tile is an element of a map
/// </summary>
public class Tile : MonoBehaviour
{

    #region Accessibility
    /// <summary>
    /// is accessible by a ground unit
    /// </summary>
    public bool Accessible_GroundUnit;
    // is accessible by a water unit
    public bool Accessible_WaterUnit;
    // is accessible by an Air unit
    public bool Accessible_AirUnit;
    // is accessible if unit is a vehicle
    public bool Accessible_Vehicule;
    #endregion

    #region Visual
    public UnityEngine.Color MovePossibleColor;
    #endregion

    #region Bonus
    /// <summary>
    /// Attack bonus applied when unit is on this tile
    /// </summary>
    public int AttackBonus;
    /// <summary>
    /// Defense bonus applied when unit is on this tile
    /// </summary>
    public int DefenseBonus;
    #endregion

    #region Private Memebrs
    /// <summary>
    /// Sprite render properties of this tile
    /// </summary>
    private SpriteRenderer m_render;
    /// <summary>
    /// Position of the tile in the grid
    /// </summary>
    private Point m_gridPosition;
    /// <summary>
    /// Manager of the current game played
    /// </summary>
    private Game m_game;
    #endregion

    #region Game tags
    /// <summary>
    /// tagged as move possible during a game move
    /// </summary>
    private bool m_tagMovePossible = false;
    #endregion

    #region Public Functions
    /// <summary>
    ///  Sets the place of the tile in the grid position
    /// </summary>
    /// <param name="ai_position">Position to set tile to</param>
    public void setGridPosition(Point ai_position) { m_gridPosition = ai_position;  }
    /// <summary>
    /// Get the position of this tile on the grid
    /// </summary>
    /// <returns>Point representing this position</returns>
    public Point getGridPosition() { return m_gridPosition; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<SpriteRenderer>();
        m_game = FindObjectOfType<Game>();
        // check for UnityConstruction values
        if (MovePossibleColor == null)
        {
            print("ERROR : No MovePossibleColor provided for object Game");
            throw new System.NullReferenceException("MovePossibleColor is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Functions
    /// <summary>
    /// Occurs when clicked on tile
    /// </summary>
    public void OnMouseDown()
    {
        m_game.onSelectedTile(this);
    }

    /// <summary>
    /// Sets the tile as accessible
    /// </summary>
    public void SetAsAccessible()
    {
        m_tagMovePossible = true;
        m_render.color = MovePossibleColor;
    }

    /// <summary>
    /// Indicates if this tile is accessible for this type of unit
    /// Only cares about type, not range of the unit
    /// </summary>
    /// <param name="ai_unit">Unit you want to know if it has access to </param>
    /// <returns>True if unit has acces to this type of tile, false otherwise</returns>
    public bool isAccessibleForUnitType(Unit ai_unit)
    {
        print("Temporaire, non terminé");
        return true;
    }

    /// <summary>
    /// Resets game turn actions for this tile
    /// </summary>
    public void ResetTileActions()
    {
        m_tagMovePossible = false;
        m_render.color = UnityEngine.Color.white;
    }

    /// <summary>
    /// Acces to move possible tag
    /// </summary>
    /// <returns>the current state of the tag</returns>
    public bool isTaggedAccessible()
    {
        return m_tagMovePossible;
    }
    #endregion
}
