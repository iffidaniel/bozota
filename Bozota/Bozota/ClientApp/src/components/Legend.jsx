import React from 'react';
import './Legend.css';
import './GameMap.css';

export const Legend = () => {
  const legendItems = [
    'empty',
    'health',
    'ammo',
    'wall',
    'bomb',
    'player',
    'bullet',
    'fire',
    'materials',
    'error',
  ];

  return (
    <div className='Legend_outer'>
      <h1>Legend</h1>
      {legendItems.map((item) => {
        return (
          <div className='Legend_inner'>
            <span className={`GameMap_cell_${item} GameMap_cell`} />
            <h2 className='Legend_title'>
              {item.charAt(0).toUpperCase() + item.slice(1)}
            </h2>
          </div>
        );
      })}
    </div>
  );
};
