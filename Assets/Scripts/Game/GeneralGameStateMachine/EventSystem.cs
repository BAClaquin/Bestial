using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using TileSelectedEvent = StateMachine.Event<EventEnum, Tile>;
    using UnitSelectedEvent = StateMachine.Event<EventEnum, Unit>;
    using AnimationIsOverEvent = StateMachine.Event<EventEnum, bool>;

    public class EventSystem : StateMachine.SingleEventSystem<EventEnum>
    {

        private TileSelectedEvent m_tileSelectedEvent;
        private UnitSelectedEvent m_unitSelectedEvent;
        private AnimationIsOverEvent m_animationIsOverEvent;

        public EventSystem()
        {
        }


        public void RaiseTileSelctedEvent(Tile ai_tile)
        {
            m_tileSelectedEvent = new TileSelectedEvent(EventEnum.TILE_SELECTED, ai_tile);
            RaiseEvent(m_tileSelectedEvent);
        }


        public void RaiseUnitSelectedEvent(Unit ai_unit)
        {
            m_unitSelectedEvent = new UnitSelectedEvent(EventEnum.UNIT_SELECTED, ai_unit);
            RaiseEvent(m_unitSelectedEvent);
        }

        public void RaiseAnimationIsOverEvent()
        {
            m_animationIsOverEvent = new AnimationIsOverEvent(EventEnum.ANIMATION_IS_OVER, true);
            RaiseEvent(m_animationIsOverEvent);
        }

        public Tile ConsumeTileSelectedEvent()
        {
            ConsumeEvent(m_tileSelectedEvent);
            return m_tileSelectedEvent.getAssociatedData();
        }

        public Unit ConsumeUnitSelectedEvent()
        {
            ConsumeEvent(m_unitSelectedEvent);
            return m_unitSelectedEvent.getAssociatedData();
        }

        public Tile getTileSelectedEventAssociatedData()
        {
            return m_tileSelectedEvent.getAssociatedData();
        }

        public Unit getUnitSelectedEventAssociatedData()
        {
            return m_unitSelectedEvent.getAssociatedData();
        }


    }
}
