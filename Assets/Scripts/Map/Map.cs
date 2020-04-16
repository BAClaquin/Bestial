using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System;

public class Map : MonoBehaviour
{
    #region Public Members
    /// <summary>
    /// Size of a tile
    /// </summary>
    public float TileSize;
    #endregion

    #region Private Members
    Point m_size;
    /// <summary>
    /// Delta to use to transforme a position to an index in our 2D map array
    /// </summary>
    PointF m_positionToIndex;
    /// <summary>
    /// 2D array of unique tile
    /// </summary>
    Tile[,] m_tileMap;
    /// <summary>
    /// Have tiles been initialized
    /// </summary>
    bool m_tilesInitialized;
    /// <summary>
    /// Have tiles been initialized
    /// </summary>
    bool m_unitsInitialized;
    /// <summary>
    /// Liste of units in the map
    /// </summary>
    List<Unit> w_listOfUnit;
    #endregion

    #region Constructors
    Map()
    {
        m_tilesInitialized = false;
        m_unitsInitialized = false;
        w_listOfUnit = new List<Unit>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // initialising tiles based on what's present on the scene
        m_tilesInitialized = InitialiseTiles();
        if(!m_tilesInitialized)
        {
            print("ERROR : tiles not correctly initialized : won't go further.");
            return;
        }
        // initialise units based on what's present on the scene
        m_unitsInitialized = InitialiseUnits();
    }



    // Update is called once per frame
    void Update()
    {
       
    }

    #region Private Functions
    /// <summary>
    ///  Init map on tiles based on tiles existing on the scene
    /// </summary>
    /// <returns>True if correctly init, false otherwise</returns>
    private bool InitialiseTiles()
    {
        Vector3 w_min = new Vector3();
        Vector3 w_max = new Vector3();
        bool w_firstIter = true;
        bool w_correctlyInitialized = true; // will be set to false when encoutering errors

        // get all tiles in the scene
        Tile[] w_tiles = FindObjectsOfType<Tile>();

        // for each tiles on the map
        foreach (Tile w_tile in w_tiles)
        {
             // store min an max value fore init
            if (w_firstIter)
            {
                w_min = w_tile.transform.position;
                w_max = w_min;
                w_firstIter = false;
            }

            // update min position value if found
            if(w_tile.transform.position.x <= w_min.x && w_tile.transform.position.y <= w_min.y)
            {
                w_min = w_tile.transform.position;
            }
            // update max position value if found
            else if(w_tile.transform.position.x >= w_max.x && w_tile.transform.position.y >= w_max.y)
            {
                w_max = w_tile.transform.position;
            }
        }

        // computing map size
        Vector3 w_size = w_max - w_min;
        m_size.X = (int) (w_size.x + 1);
        m_size.Y = (int) (w_size.y + 1);

        // creating a map of tile with the good size
        m_tileMap = new Tile[m_size.X, m_size.Y];

        // store the transformation to apply from a X Y position to a X Y array index
        m_positionToIndex = new PointF(w_min.x, w_min.y);

        // Browse again each tile to place them in our 2D Map
        Point w_index;
        foreach (Tile w_tile in w_tiles)
        {
            // get index based on tile position
            w_index = convertToGridPosition(w_tile.transform.position);
            // check if index is free
            if (m_tileMap[w_index.X, w_index.Y] != null)
            {
                print("ERROR : tile is full for position = " + w_index);
                w_correctlyInitialized = false;
            }
            // add tile to index
            else
            {
                m_tileMap[w_index.X, w_index.Y] = w_tile;
            }
        }

        // check for each position if there is a tile
        for(int ix = 0; ix < m_size.X; ix++)
        {
            for (int iy = 0; iy < m_size.Y; iy++)
            {
                if (m_tileMap[ix,iy] == null)
                {
                    print("ERROR : tile [" + ix + "," + iy + "] is NULL");
                    w_correctlyInitialized = false;
                }
            }
        }

        print("NOMBRE DE TUILES " + m_size);

        return w_correctlyInitialized;
    }

    /// <summary>
    /// Initialise Units based on existing units on the scene
    /// Map initialising must have been called before
    /// </summary>
    ///  /// <returns>True if correctly init, false otherwise</returns>
    private bool InitialiseUnits()
    {
        bool w_correctlyInitialized = true; // will be set to false when encoutering errors
        // get all tiles in the scene
        Unit[] w_units = FindObjectsOfType<Unit>();

        foreach(Unit w_unit in w_units)
        {
            // adding unit to list
            w_listOfUnit.Add(w_unit);
            // setting unit position on the grid
            w_unit.setGridPosition( convertToGridPosition(w_unit.transform.position) );

            //w_unit.transform.Translate(Vector2.down * 2);
        }

        return w_correctlyInitialized;
    }

    /// <summary>
    /// Transforms a provided position to the matching index in the 2D vector of the map
    /// </summary>
    /// <param name="ai_position">Position of the element (Z will be ignored)</param>
    /// <returns>2D index position</returns>
    private Point convertToGridPosition(Vector3 ai_position)
    {
        // check that TileSize value has correctly been set
        if(TileSize <= 0)
        {
            throw new Exception("TileSize <= 0 : value makes no sense");
        }
        // half size of a tile
        float w_halfTile = TileSize / 2;

        // set the position in the Map frame
        PointF w_IndexPositionF = new PointF(ai_position.x - m_positionToIndex.X, ai_position.y - m_positionToIndex.Y);
        // position as trucated int value (ex 1.5 --> 1)
        Point w_IndexPosition = new Point((int) Math.Truncate(w_IndexPositionF.X), (int) Math.Truncate(w_IndexPositionF.Y));

        // check for each bound to determine if it has to be adjusted
        // Manage Y axis
        if ( (w_IndexPositionF.X - w_IndexPosition.X) < -w_halfTile) // left bound
        {
            w_IndexPosition.X -= 1;
        }
        else if ( ( w_IndexPositionF.X - w_IndexPosition.X) > w_halfTile) // right bound
        {
            w_IndexPosition.X += 1;
        }
        // Manage Y axis
        if ( (w_IndexPositionF.Y - w_IndexPosition.Y) < -w_halfTile) // lower bound
        {
            w_IndexPosition.Y -= 1;
        }
        else if ( (w_IndexPositionF.Y - w_IndexPosition.Y) > w_halfTile) // upper bound
        {
            w_IndexPosition.Y += 1;
        }

        // check if computed point is in bounds
        if (!isInBounds(w_IndexPosition))
        {
            print(ai_position + " -->" + w_IndexPosition);
            throw new IndexOutOfRangeException("Computed Position OOR : " + w_IndexPosition);
        }

        // return result
        return w_IndexPosition;
    }

    /// <summary>
    /// Indicates if given indexes are in bound of the map
    /// </summary>
    /// <param name="ai_indexPosition">Index you want to check</param>
    /// <returns>True if in bounds false otherwise</returns>
    bool isInBounds(Point ai_indexPosition)
    {
        return (ai_indexPosition.X >= 0 && ai_indexPosition.Y >= 0 && ai_indexPosition.X < m_size.X && ai_indexPosition.Y < m_size.Y);
    }
    #endregion
}
