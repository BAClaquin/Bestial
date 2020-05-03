using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StateMachine
{
    public class EventBase<TEventEnum>
        where TEventEnum : System.Enum
    {
        public TEventEnum ID { get; private set; }
        public bool Consumed { get; private set; }

        public EventBase(TEventEnum ai_ID)
        {
            ID = ai_ID;
            Consumed = false;
        }

        public void ConsumeEvent()
        {
            Consumed = true;
        }
    }

    public class Event<TEventEnum,TAssociatedData> : EventBase<TEventEnum>
        where TEventEnum : System.Enum
    {
        private TAssociatedData m_data;

        public Event(TEventEnum ai_id, TAssociatedData ai_data) : base(ai_id)
        {
            m_data = ai_data;
        }

        public TAssociatedData getAssociatedData()
        {
            return m_data;
        }
    }

    /// <summary>
    /// Base class to use for single event system
    /// </summary>
    /// <typeparam name="TEventEnum">Enum of events managed by this class</typeparam>
    public abstract class SingleEventSystem<TEventEnum>
    where TEventEnum : System.Enum
    {
        #region Private Members
        EventBase<TEventEnum> m_lastRaisedEvent;
        #endregion

        /// <summary>
        /// This function should be call by enhariting class to indicate event received has been raised
        /// </summary>
        /// <param name="ai_eventID">Event base to raise</param>
        private void RaiseEvent(EventBase<TEventEnum> ai_eventBase)
        {
            m_lastRaisedEvent = ai_eventBase;
        }

        /// <summary>
        /// This function should be call by enhariting class to indicate event has been consumed
        /// </summary>
        /// <param name="ai_eventBase"></param>
        private void ConsumeEvent(EventBase<TEventEnum> ai_eventBase)
        {
            if(m_lastRaisedEvent.ID.Equals(ai_eventBase.ID))
            {
                m_lastRaisedEvent.ConsumeEvent();
            }
        }

        /// <summary>
        /// Tells if an event has occured and hasn't been consumed
        /// </summary>
        /// <param name="ai_eventID">ID of the event you ask if it's been raised</param>
        /// <returns>true if occured and not comnsumed, false otherwise</returns>
        public bool HasEventOccured(TEventEnum ai_eventID)
        {
            return (m_lastRaisedEvent.ID.Equals(ai_eventID) && !m_lastRaisedEvent.Consumed);
        }
    }
}