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
    List<Unit> m_listOfUnits;
    #endregion

    #region Constructors
    public Map()
    {
        m_tilesInitialized = false;
        m_unitsInitialized = false;
        m_listOfUnits = new List<Unit>();
    }
    #endregion


    #region Public Functions
    /// <summary>
    /// Display all available tiles where unit can move
    /// </summary>
    /// <param name="ai_unit">Unit you want to move</param>
    public void DisplayAvailableMoves(Unit ai_unit)
    {
        // first dumb implement
        List<Point> w_accessiblePositions = getPositionsWithin(ai_unit.getGridPosition(), 1);

        foreach (var w_point in w_accessiblePositions)
        {
            m_tileMap[w_point.X, w_point.Y].SetAvailableMove();
        }
    }

    /// <summary>
    /// Resets tiles that where prevously set as possible move
    /// </summary>
    public void ResetAvailableMoves()
    {
        foreach(Tile w_tile in m_tileMap)
        {
            w_tile.ResetTileActions();
        }
    }

    /// <summary>
    /// Function to call on next turn
    /// </summary>
    public void ResetUnits()
    {
        // browse all units
        foreach(Unit w_unit in m_listOfUnits)
        {
            w_unit.ResetAvailability();
        }
    }
    #endregion

    #region UI Functions
    // Start is called before the first frame update
    public void Start()
    {
        // check for UnityConstruction values
        if (TileSize <= 0)
        {
            print("ERROR : TileSize <= 0");
            throw new System.ArgumentOutOfRangeException("TileSize <= 0");
        }

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
    #endregion

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
                // store the position inside the tile
                w_tile.setGridPosition(new Point(w_index.X, w_index.Y));
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
            m_listOfUnits.Add(w_unit);
            // setting unit position on the grid
            w_unit.setGridPosition( convertToGridPosition(w_unit.transform.position) );
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

    /// <summary>
    /// get a list of possition accessible of a given position with a provided range
    /// </summary>
    /// <param name="ai_basePosition">Position to start from</param>
    /// <param name="ai_range">Range of access</param>
    /// <returns></returns>
    List<Point> getPositionsWithin(Point ai_basePosition, int ai_range)
    {
        List<Point> w_result = new List<Point>();

        // DUMB IMPLEM
        Point w_up = new Point(ai_basePosition.X, ai_basePosition.Y);
        Point w_down = new Point(ai_basePosition.X, ai_basePosition.Y);
        Point w_left = new Point(ai_basePosition.X, ai_basePosition.Y);
        Point w_right = new Point(ai_basePosition.X, ai_basePosition.Y);
        w_up.Y += 1; w_down.Y -= 1; w_left.X -= 1; w_right.X +=1;


        if (isInBounds(w_up)) { w_result.Add(w_up); }
        if (isInBounds(w_down)) { w_result.Add(w_down); }
        if (isInBounds(w_left)) { w_result.Add(w_left); }
        if (isInBounds(w_right)) { w_result.Add(w_right); }

        return w_result;
    }
    #endregion
}
