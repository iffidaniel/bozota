import React, { useState } from 'react';
import { observer } from 'mobx-react-lite';
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

export const GameMap = observer(({ game }) => {
  const [mapArray, setMapArray] = useState(null);
  setInterval(() => {
    game.update();
    setMapArray(game.state.map);
  }, 1000);

  return (
    <div className='gameMapContainer'>
      {mapArray && (
        <>
          <h2>Battle Map</h2>
          <div className='gameMap'>
            {mapArray.map((row, ri) => {
              return (
                <div className='row' key={row}>
                  {row.map((column, ci) => {
                    const cellKey = ri + 2 * ci + 1; // makes keys unique
                    return renderItem(column, cellKey);
                  })}
                </div>
              );
            })}
          </div>
        </>
      )}
    </div>
  );
});
