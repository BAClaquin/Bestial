using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Trace level in order of importance
/// </summary>
public enum TraceLevel
{
    ERROR, // When something wrong should not happen
    WARNING, // when something is dangerous but game can continue
    INFO1, // 1st level of information (High Level Info)
    INFO2, // 2nd level of information (More Detailed Information)
    DEBUG // Debug information when needed
}

public class Tracer : MonoBehaviour
{
    #region Public Unity Members
    [Header(UnityHeaders.Developp)]
    public TraceLevel Level;
    #endregion

    #region Private Memebers
    /// <summary>
    /// Singleton access instance
    /// </summary>
    private static Tracer m_instance;
    #endregion

    #region Singleton
    /// <summary>
    /// Singleton static acces for instance
    /// </summary>
    public static Tracer Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Tracer>();
                if (m_instance == null)
                {
                    print("ERROR : No instance added to Game !");
                }
            }
            return m_instance;
        }
    }
    #endregion

    #region Public Functions
    /// <summary>
    /// If trace level is suffiscient to be displayed, displays the trace
    /// </summary>
    /// <param name="ai_level">Level of importance of the trace</param>
    /// <param name="ai_msg">Message to display</param>
    public void Trace(TraceLevel ai_level, string ai_msg,
        [System.Runtime.CompilerServices.CallerMemberName] string ai_callerFunction = "")
    {
        var w_methodInfo = new StackTrace().GetFrame(1).GetMethod();
        string w_callerClass = w_methodInfo.ReflectedType.Name;

        // display trace by order of importance

        if (Level >= ai_level)
        {
            print(  "[" + ai_level.ToString() + "]" +
                    " [From :  " + w_callerClass + "." + ai_callerFunction + "] : " +
                    ai_msg);
        }
        // else trace level not displayed
    }
    #endregion
}
