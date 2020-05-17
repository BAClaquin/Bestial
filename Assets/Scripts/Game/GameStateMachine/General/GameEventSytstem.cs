using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateMachine
{

    using TileSelectedEvent = StateMachine.Event<EventEnum, Tile>;
    using UnitSelectedEvent = StateMachine.Event<EventEnum, Unit>;
    using MoveIsOverEvent = StateMachine.Event<EventEnum, bool>;

    public class GameEventSytstem : StateMachine.SingleEventSystem<EventEnum>
    {

        private TileSelectedEvent m_tileSelectedEvent;
        private UnitSelectedEvent m_unitSelectedEvent;
        private MoveIsOverEvent m_moveIsOver;

        public GameEventSytstem()
        {
        }


        #region Tile Selected
        public void RaiseTileSelctedEvent(Tile ai_tile)
        {
            m_tileSelectedEvent = new TileSelectedEvent(EventEnum.TILE_SELECTED, ai_tile);
            RaiseEvent(m_tileSelectedEvent);
        }

        public Tile ConsumeTileSelectedEvent()
        {
            ConsumeEvent(m_tileSelectedEvent);
            return m_tileSelectedEvent.getAssociatedData();
        }

        public Tile GetTileSelectedData()
        {
            return m_tileSelectedEvent.getAssociatedData();
        }
        #endregion


        #region Unit Selected
        public void RaiseUnitSelectedEvent(Unit ai_unit)
        {
            m_unitSelectedEvent = new UnitSelectedEvent(EventEnum.UNIT_SELECTED, ai_unit);
            RaiseEvent(m_unitSelectedEvent);
        }

        public Unit ConsumeUnitSelectedEvent()
        {
            ConsumeEvent(m_unitSelectedEvent);
            return m_unitSelectedEvent.getAssociatedData();
        }

        public Unit GetUnitSelectedData()
        {
            return m_unitSelectedEvent.getAssociatedData();
        }
        #endregion


        #region Move Is Over
        public void RaiseMoveOverEvent()
        {
            m_moveIsOver = new MoveIsOverEvent(EventEnum.MOVE_IS_OVER, true);
            RaiseEvent(m_moveIsOver);
        }

        public void ConsumeMoveOver()
        {
            ConsumeEvent(m_moveIsOver);
        }
        #endregion
    }
}
