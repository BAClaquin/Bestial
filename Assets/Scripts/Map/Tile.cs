using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
