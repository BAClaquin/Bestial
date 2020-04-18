using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

/// <summary>
/// Tile is an element of a map
/// </summary>
public class Tile : MonoBehaviour
{

    #region Unity Public Members
    [Header(UnityHeaders.Gameplay)]
    /// <summary>
    /// Field of action for which tile is accessible
    /// </summary>
    public List<Field> AccessibleFieldOfOperation;
    /// <summary>
    /// Unit types for which tiles is accessible
    /// </summary>
    public List<UnitType> AccessibleUnitType;

    [Header(UnityHeaders.Visuals)]
    public UnityEngine.Color MovePossibleColor;
    #endregion

    #region Private Members
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

    #region Game tags
    /// <summary>
    /// tagged as move possible during a game move
    /// </summary>
    private bool m_tagMovePossible = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        retreiveSceneComponents();
        assertUserDefinedValues();
    }

    /// <summary>
    /// Retreives components in the scene
    /// </summary>
    private void retreiveSceneComponents()
    {
        m_render = GetComponent<SpriteRenderer>();
        if (m_render == null)
        {
            throw new System.NullReferenceException("m_render is null");
        }

        // game object
        m_game = FindObjectOfType<Game>();
        if (m_game == null)
        {
            throw new System.NullReferenceException("m_game is null");
        }
    }

    private void assertUserDefinedValues()
    {
        // check for UnityConstruction values
        if (MovePossibleColor == null)
        {
            throw new System.NullReferenceException("MovePossibleColor is null");
        }
        // check for UnityConstruction values
        if (AccessibleUnitType == null)
        {
            throw new System.NullReferenceException("AccessibleUnitType is null");
        }
        if(AccessibleFieldOfOperation == null)
        {
            throw new System.NullReferenceException("AccessibleFieldOfAction is null");
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
        // must be accessible for unit type and unit field of operation
        return AccessibleUnitType.Contains(ai_unit.UnitType) && AccessibleFieldOfOperation.Contains(ai_unit.FieldOfOperation);
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
