﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StateMachine;

namespace GameStateMachine
{
    /// <summary>
    /// Game event system for emiter use
    /// </summary>
    public interface IGameEventEmitter
    {
        IDataEventEmitter<Tile> GetTileSelectedEmitter();
        IDataEventEmitter<Unit> GetUnitSelectedEmitter();
        IBasicEventEmitter GetMoveIsOverEmitter();
    }

    /// <summary>
    /// Game event system for consumer use
    /// </summary>
    public interface IGameEventConsumer
    {
        IDataEventConsumer<Tile> TileSelectedConsumer();
        IDataEventConsumer<Unit> UnitSelectedConsumer();
        IBaseEventConsumer MoveIsOverConsumer();
    }

    public class GameEventSytstem : StateMachine.BaseEventSystem, IGameEventEmitter, IGameEventConsumer
    {

        private DataEvent<Tile> m_tileSelected;
        private DataEvent<Unit> m_unitSelected;
        private BasicEvent m_moveIsOver;

        public GameEventSytstem()
        {
            // tile selected
            m_tileSelected = new DataEvent<Tile>();
            m_possibleEvents.Add(m_tileSelected);

            // unit selected
            m_unitSelected = new DataEvent<Unit>();
            m_possibleEvents.Add(m_tileSelected);

            // moveIsOver
            m_moveIsOver = new BasicEvent();
            m_possibleEvents.Add(m_moveIsOver);
        }

        #region Accessor for consumer and emiter
        // MOVE IS OVER
        public IBasicEventEmitter GetMoveIsOverEmitter()
        {
            return m_moveIsOver;
        }
        public IBaseEventConsumer MoveIsOverConsumer()
        {
            return m_moveIsOver;
        }

        // TILE SELECTED
        public IDataEventEmitter<Tile> GetTileSelectedEmitter()
        {
            return m_tileSelected;
        }
        public IDataEventConsumer<Tile> TileSelectedConsumer()
        {
            return m_tileSelected;
        }

        // UNIT SELECTED
        public IDataEventConsumer<Unit> UnitSelectedConsumer()
        {
            return m_unitSelected;
        }
        public IDataEventEmitter<Unit> GetUnitSelectedEmitter()
        {
            return m_unitSelected;
        }
        #endregion
    }
}
