using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Unit[] Units;
    public string Name;

    /// <summary>
    /// Color of unit when selected
    /// </summary>
    public UnityEngine.Color UnitColor;

    // Start is called before the first frame update
    void Start()
    {
        Array.ForEach(Units, Unit => Unit.SetPlayerColor(UnitColor));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool HasUnit(Unit m_selectedUnit)
    {
        return Array.IndexOf(Units, m_selectedUnit) != -1;
    }
}
