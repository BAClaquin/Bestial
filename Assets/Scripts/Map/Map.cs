using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;

public class Map : MonoBehaviour
{
    #region Public Members
    #endregion

    #region Private Members
    Point m_size;
    /// <summary>
    /// Delta to use to transforme a position to an index in our 2D map array
    /// </summary>
    PointF m_positionToIndex;
    Tile[,] m_tileMap;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        ListTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Private functions
    /// <summary>
    /// Init map on tiles based on tiles existing on the scene
    /// </summary>
    private void ListTiles()
    {
        Vector3 w_min = new Vector3();
        Vector3 w_max = new Vector3();
        bool w_firstIter = true;

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
            w_index = convertPositionToIndexes(w_tile.transform.position);
            // check if index is free
            if (m_tileMap[w_index.X, w_index.Y] != null)
            {
                print("ERROR : tile is full for position = " + w_index);
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
                }
            }
        }
    }

    /// <summary>
    /// Transforms a provided position to the matching index in the 2D vector of the map
    /// </summary>
    /// <param name="ai_position">Position of the element (Z will be ignored)</param>
    /// <returns>2D index position</returns>
    private Point convertPositionToIndexes(Vector3 ai_position)
    {
        return new Point( (int) (ai_position.x - m_positionToIndex.X), (int) (ai_position.y - m_positionToIndex.Y)  );
    }
    #endregion
}
