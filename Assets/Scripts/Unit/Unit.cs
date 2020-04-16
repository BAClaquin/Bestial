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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<SpriteRenderer>();
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
