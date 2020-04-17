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
        // the tile of the unit
        List<Tile> w_unitTile = new List<Tile>();
        w_unitTile.Add(m_tileMap[ai_unit.getGridPosition().X, ai_unit.getGridPosition().Y]);
        // all accessible tiles
        List<Tile> w_allTilesAccessible = new List<Tile>();
        // compute and mark all accessible tiles for this unit
        computeAccessibleTiles(ai_unit, ai_unit.Range, w_unitTile, w_allTilesAccessible);
    }

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
        // the tile of the unit
        List<Tile> w_unitTile = new List<Tile>();
        w_unitTile.Add(m_tileMap[ai_unit.getGridPosition().X, ai_unit.getGridPosition().Y]);
        // all accessible tiles
        List<Tile> w_allTilesAccessible = new List<Tile>();
        // compute and mark all accessible tiles for this unit
        computeAccessibleTiles(ai_unit, ai_unit.MovementRange, w_unitTile, w_allTilesAccessible);
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
    /// Computes all accessible tiles from a list of tiles and provides result in aio_globalAccessibleTiles
    /// Warning : this function is recursive
    /// </summary>
    /// <param name="ai_unit">Unit for which you check accessibility</param>
    /// <param name="ai_remainningRange">Range remaining for accesss computation</param>
    /// <param name="ai_fromTiles">List of tiles you compute your accessibility from</param>
    /// <param name="aio_globalAccessibleTiles">Result of all accessible tiles</param>
    private void computeAccessibleTiles(Unit ai_unit, int ai_remainningRange, List<Tile> ai_fromTiles, List<Tile> aio_globalAccessibleTiles)
    {
        // if the range remaininfg is suffiscient to move
        if (ai_remainningRange > 0)
        {
            // create a list of new tiles accessible
            List<Tile> w_newAccessibleTiles = new List<Tile>();
            // for all tiles to start from
            foreach (Tile w_tile in ai_fromTiles)
            {
                // look for neighbours (with range of 1)
                List<Point> w_neighbours = getPositionsWithin(w_tile.getGridPosition(), 1);
                // for each of these new neighbours
                foreach (Point w_neighbourPoint in w_neighbours)
                {
                    Tile w_currentTile = m_tileMap[w_neighbourPoint.X, w_neighbourPoint.Y];
                    // if the tile has not already been tagged as accessible
                    // checked in separate if to avoid multiple computations)
                    if (! w_currentTile.isTaggedAccessible())
                    {
                        // if the tile is accessible for the provided type of unit
                        if (w_currentTile.isAccessibleForUnitType(ai_unit))
                        {
                            // we have a new accessible tile to add
                            w_newAccessibleTiles.Add(w_currentTile);
                            // once added, set it as available which will mark it as available
                            w_currentTile.SetAsAccessible();
                        }
                    }
                }
            }

            // adding all found new accessible tiles to all accessible tiles
            aio_globalAccessibleTiles.AddRange(w_newAccessibleTiles);

            // if remainning range to compute
            if (ai_remainningRange > 1)
            {
                // compute again new accessible tiles from new tiles found as accessible
                computeAccessibleTiles(ai_unit, ai_remainningRange - 1, w_newAccessibleTiles, aio_globalAccessibleTiles);
            }
        }
        // here nothing to do end of function
    }

    /// <summary>
    ///  Init map on tiles based on tiles existing on the scene
    /// </summary>
    /// <returns>True if correctly init, false otherwise</returns>
    private bool InitialiseTiles()
    {
        // get all tiles in the scene
        Tile[] w_tiles = FindObjectsOfType<Tile>();

        // compute caracteristics for map copsed of provided tiles
        initGridCaracteristics(w_tiles);

        // fill the grid with each tiles based on previously computed caracteristics
        return fillCaracterizedGrid(w_tiles);
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
    /// <returns>List of eligible points</returns>
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

    /// <summary>
    /// Detects tiles characteristics such as :
    ///  - Size
    ///  - PositionToIndex transfoirmation
    ///  - TileMap object
    /// and store ioit in members
    /// </summary>
    void initGridCaracteristics(Tile[] ai_tiles)
    {
        // preconditions
       if(ai_tiles.Length <= 0)
        {
            throw new Exception("No tiles provided for detectTilesCharecteristics");
        }

        //init min and max value
        // safe as 0 exists here
        Vector3 w_min = ai_tiles[0].transform.position;
        Vector3 w_max = ai_tiles[0].transform.position;

        // Get min and max tile :
        foreach (Tile w_tile in ai_tiles)
        {
            // update min position value if found
            if (w_tile.transform.position.x <= w_min.x && w_tile.transform.position.y <= w_min.y)
            {
                w_min = w_tile.transform.position;
            }
            // update max position value if found
            else if (w_tile.transform.position.x >= w_max.x && w_tile.transform.position.y >= w_max.y)
            {
                w_max = w_tile.transform.position;
            }
        }

        // computing map size
        Vector3 w_size = w_max - w_min;
        m_size.X = (int)(w_size.x + 1);
        m_size.Y = (int)(w_size.y + 1);

        // creating a map of tile with the good size
        m_tileMap = new Tile[m_size.X, m_size.Y];

        // store the transformation to apply from a X Y position to a X Y array index
        m_positionToIndex = new PointF(w_min.x, w_min.y);
    }

    /// <summary>
    /// Fills the grid of tiles with provided tiles based on their position
    /// </summary>
    /// <param name="ai_tiles">Tiles composing the map</param>
    /// <returns>True if grid is correctly filled, false otherwise</returns>
    bool fillCaracterizedGrid(Tile[] ai_tiles)
    {
        // Browse each tile to place them in our 2D Map
        Point w_index;
        foreach (Tile w_tile in ai_tiles)
        {
            // get index based on tile position
            w_index = convertToGridPosition(w_tile.transform.position);
            // check if index is free
            if (m_tileMap[w_index.X, w_index.Y] != null)
            {
                print("ERROR : tile is full for position = " + w_index);
                return false;
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
        for (int ix = 0; ix < m_size.X; ix++)
        {
            for (int iy = 0; iy < m_size.Y; iy++)
            {
                if (m_tileMap[ix, iy] == null)
                {
                    print("ERROR : tile [" + ix + "," + iy + "] is NULL");
                    return false;
                }
            }
        }

        // here no issues
        return true;
    }
    #endregion
}
