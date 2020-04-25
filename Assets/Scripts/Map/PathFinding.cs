using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

/// <summary>
/// Status for a tile during a path finding function
/// </summary>
public enum PathFindingStatus
{
    NONE,
    ACCESSIBLE,
    START_TILE
}

/// <summary>
/// Tile with pathfinding metadata
/// </summary>
public class PathFindingTile
{
    public Tile Tile;
    public PathFindingStatus Status = PathFindingStatus.NONE;
    public PathFindingTile Predecessor;
    public int CostToReach;

    public PathFindingTile(Tile ai_tile)
    {

        Tile = ai_tile;
        Status = PathFindingStatus.NONE;
        Predecessor = null;
        CostToReach = 0;
    }

    public PathFindingTile()
    {
        Tile = null;
        Status = PathFindingStatus.NONE;
        Predecessor = null;
        CostToReach = 0;
    }

    public void setStartTile()
    {
        Status = PathFindingStatus.START_TILE;
        CostToReach = 0;
    }

    public void setAccessibleFrom(PathFindingTile ai_predecessor)
    {
        Status = PathFindingStatus.ACCESSIBLE;
        Predecessor = ai_predecessor;
        CostToReach = Predecessor.CostToReach + Tile.MovingCost;
    }

    public void resetMetadata()
    {
        Status = PathFindingStatus.NONE;
        Predecessor = null;
        CostToReach = 0;
    }
}

public class PathFinding
{
    PathFindingTile[,] m_pfTilesGrid;
    IPathFindingMap m_pfMap;
    List<PathFindingTile> m_lastPathFound;

    public PathFinding(IPathFindingMap ai_pfMap)
    {
        Point w_size = ai_pfMap.getSize();
        Tile[,] w_grid = ai_pfMap.getGrid();

        m_pfMap = ai_pfMap;
        m_pfTilesGrid = new PathFindingTile[w_size.X, w_size.Y];
        // fill the grid
        for(int x = 0; x < w_size.X; x++)
        {
            for (int y = 0; y < w_size.Y; y++)
            {
                m_pfTilesGrid[x, y] = new PathFindingTile(w_grid[x, y]);
            }
        }
    }


    /// <summary>
    /// Computes all accessible tiles for the unit with the shortest path for each tile
    /// </summary>
    /// <param name="ai_unit"></param>
    /// <returns></returns>
    public List<PathFindingTile> computeAllAccessibleShortestPaths(Unit ai_unit)
    {
        // reseting previous pathfinding metadata
        resetPathFindingData();
        // mark 1st tile
        m_lastPathFound = new List<PathFindingTile>();
        m_lastPathFound.Add(MapAt(ai_unit.getGridPosition()));
        m_lastPathFound[0].setStartTile();
        // compute all accessible tiles from the start one withing the range of the unit
        // each tiles will contains shortest path to it
        _computeAllAccessibleShortestPaths(ai_unit, m_lastPathFound);
        // remove tile we startedFrom
        m_lastPathFound.RemoveAt(0);
        // return result
        return m_lastPathFound;
    }

    /// <summary>
    /// Provides an already computed path to a specific tile
    /// </summary>
    /// <param name="ai_tile"></param>
    /// <returns></returns>
    public List<Tile> getComputedPathTo(Tile ai_tile)
    {
        List<Tile> w_path = new List<Tile>();
        foreach (var w_pfTile in m_lastPathFound)
        {
            if(w_pfTile.Tile == ai_tile)
            {
                PathFindingTile w_currentNode = w_pfTile;
                do
                {
                    w_path.Add(w_currentNode.Tile);
                    w_currentNode = w_currentNode.Predecessor;
                } while (w_currentNode != null);

                // set in order from begin to end
                w_path.Reverse();
                return w_path;
            }
        }

        return w_path;
    }


    /// <summary>
    /// Computes all accessible tiles for the unit
    /// With teh shortestb path for each tile
    /// And total cost to move for each tile
    /// </summary>
    /// <param name="ai_unit"></param>
    /// <param name="aio_visitedTiles"></param>
    private void _computeAllAccessibleShortestPaths(Unit ai_unit, List<PathFindingTile> aio_visitedTiles)
    {
        PathFindingTile w_source = new PathFindingTile();
        PathFindingTile w_closestDestination = new PathFindingTile();
        int w_resultCostToReach = 0;

        bool w_resultFound = false;

        // Among tiles visited
        foreach (PathFindingTile w_pfTileSource in aio_visitedTiles)
        {
            // look for neighbours (with range of 1)
            List<PathFindingTile> w_neighbours = getTilesWithin(w_pfTileSource.Tile.getGridPosition(), 1);

            // look for nearest not processed tile
            PathFindingTile wo_eligibleNeighbours = new PathFindingTile();
            if (selectNearestNeighbourWhereUnitCanMove(ai_unit,w_neighbours, out wo_eligibleNeighbours))
            {
                int w_newReachCostFound = w_pfTileSource.CostToReach + wo_eligibleNeighbours.Tile.MovingCost;
                // first found that is accessible from the unit
                if (!w_resultFound && (w_newReachCostFound <= ai_unit.MovementRange))
                {
                    w_source = w_pfTileSource;
                    w_closestDestination = wo_eligibleNeighbours;
                    w_resultCostToReach = w_newReachCostFound;
                    w_resultFound = true;
                }
                // compare with previous found, if inferior, newest closest accessible Tile
                else if (w_resultFound && w_newReachCostFound < w_resultCostToReach)
                {
                    // update pair
                    w_source = w_pfTileSource;
                    w_closestDestination = wo_eligibleNeighbours;
                    w_resultCostToReach = w_newReachCostFound;
                }
            }
        }

        // if we found a new tile with shortest distance from start
        if(w_resultFound)
        {
            // we found a result mark it
            w_closestDestination.setAccessibleFrom(w_source);
            aio_visitedTiles.Add(w_closestDestination);
            // log DEBUG
            /*Tracer.Instance.Trace(TraceLevel.DEBUG, "Found new nearest accessible tile ["+ w_closestDestination.Tile.getGridPosition()+"]" +
                "within range of {"+ w_closestDestination.CostToReach + "} to selected unit [+ " + ai_unit.getGridPosition() +"]");*/
            // start again from newly found accessible point
            _computeAllAccessibleShortestPaths(ai_unit, aio_visitedTiles);
        }
        // else nothing : end of function
    }

    /// <summary>
    /// get a list of possition accessible of a given position with a provided range
    /// </summary>
    /// <param name="ai_basePosition">Position to start from</param>
    /// <param name="ai_range">Range of access</param>
    /// <returns>List of eligible points</returns>
    List<PathFindingTile> getTilesWithin(Point ai_basePosition, int ai_range)
    {
        List<PathFindingTile> w_result = new List<PathFindingTile>();

        // DUMB IMPLEM
        Point w_up = new Point(ai_basePosition.X, ai_basePosition.Y);
        Point w_down = new Point(ai_basePosition.X, ai_basePosition.Y);
        Point w_left = new Point(ai_basePosition.X, ai_basePosition.Y);
        Point w_right = new Point(ai_basePosition.X, ai_basePosition.Y);
        w_up.Y += 1; w_down.Y -= 1; w_left.X -= 1; w_right.X += 1;

        if (isInBounds(w_up)) { w_result.Add(MapAt(w_up)); }
        if (isInBounds(w_down)) { w_result.Add(MapAt(w_down)); }
        if (isInBounds(w_left)) { w_result.Add(MapAt(w_left)); }
        if (isInBounds(w_right)) { w_result.Add(MapAt(w_right)); }

        return w_result;
    }

    /// <summary>
    /// Provides the nearest neighbour of the list of tiles for which total
    /// move cost is the lowest
    /// </summary>
    /// <param name="ai_unit">Unit to move</param>
    /// <param name="ai_from">List of tiles to check it from</param>
    /// <param name="ao_result">Result Path</param>
    /// <returns>true if neighbour found, false otherwise</returns>
    private bool selectNearestNeighbourWhereUnitCanMove(Unit ai_unit, List<PathFindingTile> ai_from, out PathFindingTile ao_result)
    {
        bool w_found = false;
        ao_result = new PathFindingTile();

        // check for tiles available
        if (ai_from.Count <= 0)
        {
            ao_result = new PathFindingTile();
            return w_found;
        }


        // browse all given tiles
        foreach (PathFindingTile w_pfTile in ai_from)
        {
            if( m_pfMap.IsPositionFree(w_pfTile.Tile.getGridPosition()) && // tile is free (no other thing on it)
                w_pfTile.Tile.isAccessibleForUnitType(ai_unit) && // tile is accessible for this type of unit
                w_pfTile.Status == PathFindingStatus.NONE ) // tile has not already been processed
            {
                if(!w_found || // new accessible tile : set as first element found
                    w_pfTile.Tile.MovingCost <= ao_result.Tile.MovingCost)  // new element found, select it if it is nearer previously tile found
                {
                    w_found = true;
                    ao_result = w_pfTile;
                }
            }
        }

        // tell if we found a minrange that was not already processed
        return w_found;
    }

    /// <summary>
    /// Acces to map tile with a Point
    /// </summary>
    /// <param name="ai_point">X,Y</param>
    /// <returns>Tile at position</returns>
    private PathFindingTile MapAt(Point ai_point)
    {
        return m_pfTilesGrid[ai_point.X, ai_point.Y];
    }

    /// <summary>
    /// Indicates if given indexes are in bound of the map
    /// </summary>
    /// <param name="ai_indexPosition">Index you want to check</param>
    /// <returns>True if in bounds false otherwise</returns>
    bool isInBounds(Point ai_indexPosition)
    {
        return (ai_indexPosition.X >= 0 && ai_indexPosition.Y >= 0 && ai_indexPosition.X < m_pfMap.getSize().X && ai_indexPosition.Y < m_pfMap.getSize().Y);
    }


    /// <summary>
    /// Reset computed pathfind data
    /// </summary>
    private void resetPathFindingData()
    {
        foreach(PathFindingTile w_pfTile in m_pfTilesGrid)
        {
            w_pfTile.resetMetadata();
        }
    }


}
