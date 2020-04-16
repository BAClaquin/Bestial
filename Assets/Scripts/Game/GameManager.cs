using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global Game Manager where you can manage Games
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Private Members
    /// <summary>
    /// Current Game
    /// </summary>
    Game m_game;
    /// <summary>
    /// Instance of game manager
    /// </summary>
    GameManager m_instance;
    #endregion

    #region Private Functions
    /// <summary>
    /// Singleton should not be build
    /// </summary>
    private GameManager() { }
    #endregion

    #region Public Functions
    /// <summary>
    /// Acces to the singleton class GameManager
    /// </summary>
    /// <returns></returns>
    GameManager Instance()
    {
        if(m_instance == null)
        {
            m_instance = new GameManager();
        }
        return m_instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_game = new Game();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// Provides acces to the current game being played
    /// </summary>
    /// <returns></returns>
    public Game getCurrentGame()
    {
        return m_game;
    }
    #endregion
}
