using Assets.Scripts.Game.GeneralGameStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
{

    using TileSelectedEvent = StateMachine.Event<EventEnum, Tile>;
    using UnitSelectedEvent = StateMachine.Event<EventEnum, Unit>;

    public class GameEventSystem : SingleEventSystem<EventEnum>
    {

        private TileSelectedEvent m_tileSelectedEvent;
        private UnitSelectedEvent m_unitSelectedEvent;

        public GameEventSystem()
        {
        }


        public void RaiseTileSelctedEvent(Tile ai_tile)
        {
            m_tileSelectedEvent = new TileSelectedEvent(EventEnum.TILE_SELECTED, ai_tile);
            RaiseEvent(m_tileSelectedEvent);
        }


        public void RaiseUnitSelctedEvent(Unit ai_unit)
        {
            m_unitSelectedEvent = new UnitSelectedEvent(EventEnum.UNIT_SELECTED, ai_unit);
            RaiseEvent(m_unitSelectedEvent);
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


    }
}
