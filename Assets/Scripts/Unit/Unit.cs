using System.Drawing;
using UnityEngine;

/// <summary>
/// Class managing and unit
/// </summary>
public class Unit : MonoBehaviour
{
    #region Unity Public Members

    [Header(UnityHeaders.Gameplay)]
    /// <summary>
    /// Field of operation of this unit
    /// </summary>
    public Field FieldOfOperation;
    /// <summary>
    /// Type of this unit
    /// </summary>
    public UnitType UnitType;
    /// <summary>
    /// Range (in tiles) of this unit
    /// </summary>
    public int MovementRange;

    [Header(UnityHeaders.Visuals)]
    /// <summary>
    /// Color of unit when selected
    /// </summary>
    public UnityEngine.Color HighlightedColor;
    /// <summary>
    /// Color of unit when disabled
    /// </summary>
    public UnityEngine.Color DisabledColor;
    #endregion

    #region Private Members
    /// <summary>
    /// Position of an unit ona 2D grid
    /// </summary>
    private Point m_gridPosition;
    /// <summary>
    /// Sprite render properties of this unit
    /// </summary>
    private SpriteRenderer[] m_renders;
    /// <summary>
    /// Manager of the current game played
    /// </summary>
    private Game m_game;
    /// <summary>
    /// Indicates if unit has played during this run
    /// </summary>
    private bool m_hasMoved = false;
    /// <summary>
    /// When unit is disabled, no action is possible
    /// </summary>
    private bool m_disabled = false;
    #endregion

    #region Game tags

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
        // all renders of the unit
        m_renders = GetComponentsInChildren<SpriteRenderer>();
        if(m_renders == null)
        {
            throw new System.NullReferenceException("m_renders is null");
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
        if (HighlightedColor == null)
        {
            throw new System.NullReferenceException("HighlightedColor is null");
        }
        if (DisabledColor == null)
        {
            throw new System.NullReferenceException("DisabledColor is null");
        }
        if(MovementRange < 0)
        {
            throw new System.ArgumentOutOfRangeException("MovementRange cannot be < 0");
        }
    }


    // Update is called once per frame
    void Update()
    {

    }


    #region User Interaction Functions
    /// <summary>
    /// Occurs when user click on this unit
    /// </summary>
    private void OnMouseDown()
    {
        // no events for disbaled units
        if (!m_disabled)
        {
            m_game.onSelectedUnit(this);
        }

    }

    /// <summary>
    /// Visually highlights the unit
    /// </summary>
    public void Highlight()
    {
       ChangeSpritesColor(HighlightedColor);
    }

    /// <summary>
    /// Disable the unit
    /// </summary>
    public void Disable()
    {
        m_disabled = true;
        ChangeSpritesColor(DisabledColor);
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Resets availability of the unit
    /// </summary>
    public void ResetAvailability()
    {
        // reset played information
        m_hasMoved = false;
        m_disabled = false;
        // reset to default visual effect
        ResetVisualEffects();
    }
    /// <summary>
    /// Sets the position of this unit on a 2D grid defined by a map
    /// </summary>
    /// <param name="ai_gridPos"></param>
    public void setGridPosition(Point ai_gridPos)
    {
        m_gridPosition = ai_gridPos;
    }

    /// <summary>
    /// Acces to grid position of this unit
    /// </summary>
    /// <returns>Current grid position</returns>
    public Point getGridPosition()
    {
        return m_gridPosition;
    }

    public void moveTo(Point ai_newPosition)
    {
        // move
        Vector3 w_move = new Vector3(ai_newPosition.X - m_gridPosition.X, ai_newPosition.Y - m_gridPosition.Y, 0);

        // deplacement
        transform.Translate(w_move);

        // storring position in grid
        m_gridPosition = ai_newPosition;

        // unit has conumes its move
        m_hasMoved = true;

        ResetVisualEffects();
    }

    /// <summary>
    /// Indicates if units has moved during this turn
    /// </summary>
    /// <returns>True if played, false otherwise</returns>
    public bool hasMoved()
    {
        return m_hasMoved;
    }

    /// <summary>
    /// To unselect an unit
    /// </summary>
    public void Unselect()
    {
        // if unit isnt disabled
        if (!m_disabled)
        {
            // reset its visual effects
            ResetVisualEffects();
        }
        // else nothing to do
    }
    #endregion

    #region Private Functions
    /// <summary>
    /// Resets all visual effects on the unit
    /// </summary>
    void ResetVisualEffects()
    {
        ChangeSpritesColor(UnityEngine.Color.white);
    }

    private void ChangeSpritesColor(UnityEngine.Color color)
    {
        foreach (SpriteRenderer sprite in m_renders)
        {
            sprite.color = color;
        }
    }
    #endregion
}