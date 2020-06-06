using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class UnitView
{
    public float MoveSpeed = 5f;

    public UnityEngine.Color SelectedUnitColor = UnityEngine.Color.yellow;
    
    public UnityEngine.Color DisabledUnitColor = UnityEngine.Color.black;

    /// <summary>
    /// Sprite renderers of this unit
    /// </summary>
    internal SpriteRenderer[] m_renderers;

    /// <summary>
    /// Sprite renderers of this unit which display the player's color
    /// </summary>
    internal SpriteRenderer[] m_coloredOutfitRenderers;

    private readonly GameObject m_unitGameObject;

    // Unit gets bigger when selected
    private bool m_isBig = false;

    internal Animator m_anim;

    internal float m_selectUnitResizeScale = 1.5f;

    public UnitView(GameObject gameObject)
    {
        this.m_unitGameObject = gameObject;
    }

    public void MakeUnitBigger()
    {
        if (!m_isBig)
        {
            m_unitGameObject.transform.localScale = new Vector3(m_unitGameObject.transform.localScale.x * m_selectUnitResizeScale, m_unitGameObject.transform.localScale.x * m_selectUnitResizeScale, m_unitGameObject.transform.localScale.x * m_selectUnitResizeScale);
            m_unitGameObject.transform.position = new Vector3(m_unitGameObject.transform.position.x, m_unitGameObject.transform.position.y, -1);
            m_isBig = true;
        }
    }

    private void MakeUnitSmaller()
    {
        if (m_isBig)
        {
            m_unitGameObject.transform.localScale = new Vector3(m_unitGameObject.transform.localScale.x / m_selectUnitResizeScale, m_unitGameObject.transform.localScale.x / m_selectUnitResizeScale, m_unitGameObject.transform.localScale.x / m_selectUnitResizeScale);
            m_unitGameObject.transform.position = new Vector3(m_unitGameObject.transform.position.x, m_unitGameObject.transform.position.y, 0);
            m_isBig = false;
        }
    }

    public IEnumerator StartMovement(Point ai_newPosition, float ai_currentX, float ai_currentY)
    {
        // start walking animation
        m_anim.SetBool(UnityAnimationTags.IsWalking, true);

        float w_targetPositionX = m_unitGameObject.transform.position.x + (ai_newPosition.X - ai_currentX);
        float w_targetPositionY = m_unitGameObject.transform.position.y + (ai_newPosition.Y - ai_currentY);

        // turn sprite in proper direction
        SetDirection(m_unitGameObject.transform.position.x, w_targetPositionX);

        yield return MoveHorizontally(w_targetPositionX);
        yield return MoveVertically(w_targetPositionY);

        // stop walkin annimation
        m_anim.SetBool(UnityAnimationTags.IsWalking, false);
    }

    private IEnumerator MoveHorizontally(float ai_targetPositionX)
    {
        while (!IsCloseEnoughToTargetPosition(m_unitGameObject.transform.position.x, ai_targetPositionX))
        {
            m_unitGameObject.transform.position = Vector3.MoveTowards(m_unitGameObject.transform.position, new Vector3(ai_targetPositionX, m_unitGameObject.transform.position.y, 0), MoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveVertically(float ai_targetPositionY)
    {
        while (!IsCloseEnoughToTargetPosition(m_unitGameObject.transform.position.y, ai_targetPositionY))
        {
            m_unitGameObject.transform.position = Vector3.MoveTowards(m_unitGameObject.transform.position, new Vector3(m_unitGameObject.transform.position.x, ai_targetPositionY, 0), MoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void SetDirection(float ai_positionX, float ai_targetPositionX)
    {
        if (ai_positionX != ai_targetPositionX)
        {
            m_unitGameObject.transform.eulerAngles = ai_positionX < ai_targetPositionX ? new Vector3(0, 180, 0) : new Vector3(0, 0, 0);
        }
    }
    private bool IsCloseEnoughToTargetPosition(float ai_position, float ai_targetPosition)
    {
        return Math.Abs(ai_position - ai_targetPosition) < 0.001f;
    }

    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            MakeUnitBigger();
            ChangeSpritesColor(SelectedUnitColor);
        }
        else
        {
            MakeUnitSmaller();
            ResetVisualEffects();
        }
    }

    public void ApplyPlayerColor(UnityEngine.Color unitColors)
    {
        Array.ForEach(m_coloredOutfitRenderers, sprite => sprite.color = unitColors);
    }

    public void ApplyDisabledColor()
    {
        ChangeSpritesColor(DisabledUnitColor);
    }


    public void AssertUserDefinedValues()
    {
        // check for UnityConstruction values
        if (SelectedUnitColor == null)
        {
            throw new System.NullReferenceException("HighlightedColor is null");
        }
        if (DisabledUnitColor == null)
        {
            throw new System.NullReferenceException("DisabledColor is null");
        }
    }
    
    public void ResetVisualEffects()
    {
        ChangeSpritesColor(UnityEngine.Color.white);
    }

    private void ChangeSpritesColor(UnityEngine.Color ai_color)
    {
        Array.ForEach(m_renderers, sprite => sprite.color = ai_color);
    }



}
