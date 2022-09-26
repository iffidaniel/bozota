import React from 'react';
import './GameMap.css';

const renderItem = (id, key) => {
  if (id === 0) {
    return <span className='GameMap_cell_empty GameMap_cell' key={key} />;
  } else if (id === 1) {
    return <span className='GameMap_cell_health GameMap_cell' key={key} />;
  } else if (id === 2) {
    return <span className='GameMap_cell_ammo GameMap_cell' key={key} />;
  } else if (id === 3) {
    return <span className='GameMap_cell_wall GameMap_cell' key={key} />;
  } else if (id === 4) {
    return <span className='GameMap_cell_bomb GameMap_cell' key={key} />;
  } else if (id === 5) {
    return <span className='GameMap_cell_player GameMap_cell' key={key} />;
  } else if (id === 6) {
    return <span className='GameMap_cell_bullet GameMap_cell' key={key} />;
  } else if (id === 7) {
    return <span className='GameMap_cell_fire GameMap_cell' key={key} />;
  } else if (id === 8) {
    return <span className='GameMap_cell_materials GameMap_cell' key={key} />;
  } else {
    return <span className='GameMap_cell_error GameMap_cell' key={key} />;
  }
};

export const GameMap = ({ gameState }) => {
  return (
    <div className='GameMap_outer'>
      {gameState && (
        <>
          <div className='GameMap_inner'>
            {gameState.map.map((row, ri) => {
              return (
                <div className='row' key={ri}>
                  {row.map((column, ci) => {
                    return renderItem(column, ci);
                  })}
                </div>
              );
            })}
          </div>
        </>
      )}
    </div>
  );
};
