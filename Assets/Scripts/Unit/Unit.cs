using System.Drawing;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
    public float MoveSpeed = 0.1f;

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
    /// Player of the unit
    /// </summary>
    public Player Player { get; set; }

    /// <summary>
    /// Sprite renderers of this unit
    /// </summary>
    private SpriteRenderer[] m_renderers;

    /// <summary>
    /// Sprite renderers of this unit which display the player's color
    /// </summary>
    private SpriteRenderer[] m_coloredOutfitRenderers;

    /// <summary>
    /// Manager of the current game played
    /// </summary>
    private Game m_game;

    /// <summary>
    /// Indicates if unit has played during this turn
    /// <summary>
    private bool m_hasMoved = false;
    
    // Indicates if unit has attacked during this turn    
    private bool m_hasAttacked = false;
    /// <summary>
    /// When unit is disabled, no action is possible
    /// </summary>
    private bool m_disabled = false;

    private bool m_selected = false;
       
    private UnitView m_unitView;

    private readonly string m_coloredOutfitGameObjectTag = "playerColoredOutfit";

    #endregion

    // Start is called before the first frame update
    void Start()
    {
      
        SetGame();

        if (MovementRange< 0)
        {
            throw new System.ArgumentOutOfRangeException("MovementRange cannot be < 0");
        }

        m_unitView = GetUnitView();
        m_unitView.AssertUserDefinedValues();

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        if (Player != null)
        {
            m_unitView.ApplyPlayerColor(Player.UnitColor);
        }
        else
        {
            Tracer.Instance.Trace(TraceLevel.WARNING, "Player Null" );
        }
    }

    /// <returns>Description as a string</returns>
    public string Describe()
    {
        return "{ Type : " + UnitType.ToString() + " | Commander : " + Player.Name + " | Number : @TODO }";
    }

    public void SetPlayer(Player ai_player)
    {
        Player = ai_player;     
    }


    #region User Interaction Functions
    /// <summary>
    /// Occurs when user click on this unit
    /// </summary>
    private void OnMouseDown()
    {
       m_game.OnSelectedUnit(this);
    }

    public void SetSelected(bool ai_selected)
    {
        m_selected = ai_selected;       
    }

    public void Highlight(bool highlight)
    {
        m_unitView.Highlight(highlight);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Disable()
    {
        m_disabled = true;
        m_unitView.ApplyDisabledColor();
    }

    public bool IsDisabled()
    {
        return m_disabled;
    }

    public Point GetGridPosition()
    {
        return m_gridPosition;
    }

    #endregion

    #region Public Functions

    public void SetGridPosition (Point ai_gridPosition)
    {
        this.m_gridPosition = ai_gridPosition;
    }

    public void ResetAvailability()
    {
        // reset played information
        m_hasMoved = false;
        m_disabled = false;
        m_hasAttacked = false;
        // reset to default visual effect
        m_unitView.ResetVisualEffects();
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

 
    public void ConsumeAttack()
    {
        m_hasAttacked = true;
    }

    public bool HasConsumedAllAttacks()
    {
        return m_hasAttacked;
    }

    public IEnumerator MoveTo(List<Tile> ai_path)
    {
        foreach(Tile w_tile in ai_path)
        {
            Tracer.Instance.Trace(TraceLevel.DEBUG, "GOTO -> " + w_tile.GetGridPosition());
            // deplacement

            yield return m_unitView.StartMovement(w_tile.GetGridPosition(), m_gridPosition.X, m_gridPosition.Y);
            // storing position in grid
            m_gridPosition = w_tile.GetGridPosition();
        }
        // unit has conumes its move
        m_hasMoved = true;
    }

    /// <summary>
    /// Tells if unit has consumed at least one action
    /// </summary>
    public bool HasConsumedActions()
    {
        return m_hasAttacked || m_hasMoved;
    }

    /// <summary>
    /// Indicates if units has moved during this turn
    /// </summary>
    /// <returns>True if played, false otherwise</returns>
    public bool CanMove()
    {
        // unit can move if it has not moved and have remaining range
        // TODO : implement remaining gas
        return !m_hasMoved;
    }

    public bool CanAttack()
    {
        // TODO : implement remaining munition
        return !m_hasAttacked;
    }
    #endregion


    private void SetGame()
    {
        // game object        
        m_game = FindObjectOfType<Game>();
        if (m_game == null)
        {
            throw new System.NullReferenceException("m_game is null");
        }
    }

    /// <summary>
    /// Retreives components in the scene
    /// </summary>
    private UnitView GetUnitView()
    {
        UnitView unitView = new UnitView(this.gameObject);

        SpriteRenderer[] w_allChildrenSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();


        // filter all the renderers that must not change with the players' color
        unitView.m_renderers = GetRenderers(w_allChildrenSpriteRenderers);

        // filter all the renderers that must change with the players' color
        unitView.m_coloredOutfitRenderers = GetColoredOutfitRenderers(w_allChildrenSpriteRenderers);
          
        unitView.m_anim = GetAnim();

        unitView.DisabledUnitColor = this.DisabledColor;
        unitView.SelectedUnitColor = this.HighlightedColor;

        return unitView;
    }

    private Animator GetAnim()
    {
        Animator w_anim = GetComponentsInChildren<Animator>()[0];
        if (w_anim == null)
        {
            throw new System.NullReferenceException("m_anim is null");
        }
        w_anim.SetBool("isWalking", false);
        return w_anim;
    }

    private SpriteRenderer[] GetRenderers(SpriteRenderer[] ai_allChildrenSpriteRenderers)
    {
        SpriteRenderer[] w_renderers = Array.FindAll(ai_allChildrenSpriteRenderers, Sprite => Sprite.name != m_coloredOutfitGameObjectTag);
        if (w_renderers == null)
        {
            throw new System.NullReferenceException("m_renderers is null");
        }
        return w_renderers;
    }

    private SpriteRenderer[] GetColoredOutfitRenderers(SpriteRenderer[] ai_allChildrenSpriteRenderers)
    {
        SpriteRenderer[] w_coloredOutfitRenderers = Array.FindAll(ai_allChildrenSpriteRenderers, Sprite => Sprite.name == m_coloredOutfitGameObjectTag);
        if (w_coloredOutfitRenderers == null)
        {
            throw new System.NullReferenceException("m_coloredOutfitRenderers is null");
        }
        return w_coloredOutfitRenderers;
    }
}