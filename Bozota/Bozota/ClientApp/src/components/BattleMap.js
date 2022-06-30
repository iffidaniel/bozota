import React from 'react';
import fetchMap from '../api/fetchMap';
import './BattleMap.css';

const renderItem = (id) => {
  console.log(id);
  if (id > 4 || id === 0) {
    return <span className='empty item' />;
  } else if (id === 1) {
    return <span className='health item' />;
  } else if (id === 2) {
    return <span className='ammo item' />;
  } else if (id === 3) {
      return <span className='wall item' />;
  } else if (id === 4) {
      return <span className='bomb item' />;
  } else if (id === 5) {
      return <span className='player item' />;
  }
};

export const BattleMap = () => {
  let battleMap = fetchMap();
  return (
    <div className='battleMapContainer'>
      <h2>Battle Map</h2>
      <div className='battleMap'>
        {battleMap.map((row) => {
          return (
            <div className='row'>
              {row.map((column) => {
                return renderItem(column);
              })}
            </div>
          );
        })}
      </div>
    </div>
  );
};
