using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

/// <summary>
/// Trace level in order of importance
/// </summary>
public enum TraceLevel
{
    ERROR,
    WARNING,
    INFO,
    DEBUG
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
    /// <summary>
    /// Stack trace to display caller
    /// </summary>
    private static StackTrace m_stackTrace;
    #endregion

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
                m_stackTrace = new StackTrace();
                if (m_instance == null)
                {
                    print("ERROR : No instance added to Game !");
                }
            }
            return m_instance;
        }
    }

    /// <summary>
    /// If trace level is suffiscient to be displayed, displays the trace
    /// </summary>
    /// <param name="ai_level">Level of importance of the trace</param>
    /// <param name="ai_caller">Class + Function of the caller</param>
    /// <param name="ai_msg">Message to display</param>
    public void Trace(TraceLevel ai_level, string ai_msg)
    {
        // display trace by order of importance
        if (Level >= ai_level)
        {
            // caller of this trace function
            var w_caller = m_stackTrace.GetFrame(1).GetMethod();
            string w_callerClass = w_caller.ReflectedType.Name;
            // 
            print(  "[" + ai_level.ToString() + "]" +
                    " [From : " + w_callerClass +"."+ w_caller.Name + "] : " +
                    ai_msg);
        }
        // else trace level not displayed
    }
}
