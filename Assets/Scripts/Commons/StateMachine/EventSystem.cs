using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{

    #region BaseEvent
    /// <summary>
    /// Base event for all events from a consumer POV
    /// </summary>
    public interface IBaseEventConsumer
    {
        /// <summary>
        /// Consumes the event : it will no longer be stated as occured
        /// </summary>
        void Consume();

        /// <summary>
        /// Rejects the event : 
        /// Foprce to not occured.
        /// </summary>
        void Reject();

        /// <summary>
        /// Indicates if event has occured
        /// </summary>
        /// <returns>True if has occured, false otherwise</returns>
        bool HasOccured();
    }

    /// <summary>
    /// Base event for all events from a consumer POV
    /// </summary>
    public class BaseEventConsumer : IBaseEventConsumer
    {
        // has the event occured
        protected bool m_occured;

        /// <summary>
        /// Constructor for basic event
        /// </summary>
        public BaseEventConsumer()
        {
            m_occured = false;
        }

        /// <summary>
        /// Consumes the event : it will no longer be stated as occured
        /// </summary>
        public void Consume()
        {
            if (!m_occured)
            {
                throw new System.Exception("Trying to consume an event that has not occured");
            }
            m_occured = false;
        }

        /// <summary>
        /// Force event to not occured.
        /// </summary>
        public void Reject()
        {
            m_occured = false;
        }

        /// <summary>
        /// Indicates if event has occured
        /// </summary>
        /// <returns>True if has occured, false otherwise</returns>
        public bool HasOccured()
        {
            return m_occured;
        }
    }

    #endregion

    #region NoDataEvent

    /// <summary>
    /// For events without associated data, for Emiter
    /// </summary>
    public interface IBasicEventEmitter
    {
        /// <summary>
        /// Raise the event
        /// </summary>
        void Raise(); 
    }

    /// <summary>
    /// For events without associated data
    /// </summary>
    public class BasicEvent :  BaseEventConsumer, IBasicEventEmitter, IBaseEventConsumer
    {
        /// <summary>
        /// Raise the event
        /// </summary>
        public void Raise()
        {
            m_occured = true;
        }
    }

    #endregion

    #region DataEvent

    /// <summary>
    /// For data event from external POV
    /// </summary>
    /// <typeparam name="TAssociatedData"></typeparam>
    public interface IDataEventEmitter<TAssociatedData>
    {
        /// <summary>
        /// Raise the event as it occured
        /// </summary>
        /// <param name="ai_data">Data associated with your event</param>
        void Raise(TAssociatedData ai_data);
    }

    /// <summary>
    /// To raise events with an associated data
    /// </summary>
    /// <typeparam name="TAssociatedData"></typeparam>
    public interface IDataEventConsumer<TAssociatedData> : IBaseEventConsumer
    {
        /// <summary>
        /// Provides the data associated with the event
        /// </summary>
        /// <returns></returns>
        TAssociatedData GetAssociatedData();

        /// <summary>
        /// Provides the data associated with the event and consumes it
        /// </summary>
        /// <returns>Data associated with the event</returns>
        TAssociatedData ConsumeAssociatedData();
    }

    /// <summary>
    /// For events with an associated data
    /// </summary>
    /// <typeparam name="TAssociatedData"></typeparam>
    public class DataEvent<TAssociatedData> : BaseEventConsumer, IDataEventEmitter<TAssociatedData>, IDataEventConsumer<TAssociatedData>
    {
        /// <summary>
        /// Data associated with the event
        /// </summary>
        private TAssociatedData m_data;

        /// <summary>
        /// Raise the event as it occured
        /// </summary>
        /// <param name="ai_data">Data associated with your event</param>
        public void Raise(TAssociatedData ai_data)
        {
            m_data = ai_data;
            m_occured = true;
        }

        /// <summary>
        /// Provides the data associated with the event
        /// </summary>
        /// <returns></returns>
        public TAssociatedData GetAssociatedData()
        {
            if ( ! m_occured)
            {
                throw new System.Exception("Trying to getData for an event that has not occured");
            }
            return m_data;
        }

        /// <summary>
        /// Provides the data associated with the event and consumes event
        /// </summary>
        /// <returns></returns>
        public TAssociatedData ConsumeAssociatedData()
        {
            Consume();
            return m_data;
        }
    }

    #endregion

    #region EventSystem
    public class BaseEventSystem
    {
        /// <summary>
        /// List for all possible events that can occur;
        /// </summary>
        protected List<IBaseEventConsumer> m_possibleEvents;

        protected BaseEventSystem()
        {
            m_possibleEvents = new List<IBaseEventConsumer>();
        }

        /// <summary>
        /// Consumes all events
        /// </summary>
        public void RejectAllEvents()
        {
            foreach (var w_event in m_possibleEvents)
            {
                w_event.Reject();
            }
        }
    }
    #endregion
}