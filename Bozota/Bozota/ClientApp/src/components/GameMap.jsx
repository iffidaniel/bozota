import React, { useState } from 'react';
import { observer } from 'mobx-react-lite';
import './GameMap.css';

const renderItem = (id, key) => {
  if (id === 0) {
    return <span className='empty cell' key={key} />;
  } else if (id === 1) {
      return <span className='health cell' key={key} />;
  } else if (id === 2) {
      return <span className='ammo cell' key={key} />;
  } else if (id === 3) {
      return <span className='wall cell' key={key} />;
  } else if (id === 4) {
      return <span className='bomb cell' key={key} />;
  } else if (id === 5) {
      return <span className='player cell' key={key} />;
  } else if (id === 6) {
      return <span className='bullet cell' key={key} />;
  } else if (id === 7) {
      return <span className='fire cell' key={key} />;
  } else if (id === 8) {
      return <span className='materials cell' key={key} />;
  } else {
      return <span className='errorCell cell' key={key} />;
  }
};

export const GameMap = observer(({ mapArray }) => {

  return (
    <div className='gameMapContainer'>
      {mapArray && (
        <>
          <h2>Battle Map</h2>
          <div className='gameMap'>
            {mapArray.map((row, ri) => {
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
});
