using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

/// <summary>
/// Class managing and unit
/// </summary>
public class Unit : MonoBehaviour
{
    #region Public Members
    /// <summary>
    /// Range (in tiles) of this unit
    /// </summary>
    public int Range;
    /// <summary>
    /// Color of unit when selected
    /// </summary>
    public UnityEngine.Color HighlightedColor;
    #endregion

    #region Private Members
    /// <summary>
    /// Position of an unit ona 2D grid
    /// </summary>
    private Point m_gridPosition;
    /// <summary>
    /// Sprite render properties of this unit
    /// </summary>
    private SpriteRenderer m_render;
    /// <summary>
    /// Manager of the current game played
    /// </summary>
    private Game m_game;
    /// <summary>
    /// Indicates if unit has played during this run
    /// </summary>
    private bool m_hasMoved = false;
    #endregion

    #region Game tags

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<SpriteRenderer>();
        m_game = FindObjectOfType<Game>();

        // check for UnityConstruction values
        if (HighlightedColor == null)
        {
            print("ERROR : No Highlighted provided for object Game");
            throw new System.NullReferenceException("Highlighted is null");
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
        m_game.onSelectedUnit(this);
        // highlight unit
        Highlight();
    }
    #endregion

    #region Public Functions
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

        // desactive pour tests
        //m_hasMoved = true;
    }

    /// <summary>
    /// Indicates if units has moved during this turn
    /// </summary>
    /// <returns>True if played, false otherwise</returns>
    public bool hasMoved()
    {
        return m_hasMoved;
    }
    #endregion

    #region Private Functions
    /// <summary>
    /// Visually highlights the unit
    /// </summary>
    void Highlight()
    {
        m_render.color = HighlightedColor;
    }
    /// <summary>
    /// Resets all visual effects on the unit
    /// </summary>
    void ResetVisualEffects()
    {
        m_render.color = UnityEngine.Color.white;
    }
    #endregion
}
