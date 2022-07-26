import React, { useState, useEffect } from 'react';
import fetchGameStatus from '../api/fetchGameStatus';
import './GameMap.css';

const renderItem = (id, key) => {
  if (id === 0) {
    return <span className='empty item' key={key} />;
  } else if (id === 1) {
    return <span className='health item' key={key} />;
  } else if (id === 2) {
    return <span className='ammo item' key={key} />;
  } else if (id === 3) {
    return <span className='wall item' key={key} />;
  } else if (id === 4) {
    return <span className='bomb item' key={key} />;
  } else if (id === 5) {
    return <span className='player item' key={key} />;
  } else {
    return <span className='errorCell item' key={key} />;
  }
};

export const GameMap = () => {
  const [gameMap, setGameMap] = useState([]);
  useEffect(() => {
    fetchGameStatus().then((res) => setGameMap(res.map));
  });
  return (
    <div className='gameMapContainer'>
      <h2>Battle Map</h2>
      <div className='gameMap'>
        {gameMap.map((row, ri) => {
          return (
            <div className='row' key={row}>
              {row.map((column, ci) => {
                const cellKey = ri + 2 * ci + 1; // cell makes keys unique
                return renderItem(column, cellKey);
              })}
            </div>
          );
        })}
      </div>
    </div>
  );
};
