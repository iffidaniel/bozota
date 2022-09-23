import React from 'react';
import './Stats.css';
import { AiOutlineHeart } from 'react-icons/ai';
import { GiAmmoBox, GiMetalBoot, GiWoodenCrate } from 'react-icons/gi';

const renderCounters = (key, value, color) => {
  const counters = [];
  for (var i = 0; i < value; i++) {
    counters.push(
      <span
        key={`${key}_${i}`}
        className={`Stats_counter Stats_counter-${color}`}
      />
    );
  }
  return counters;
};

export const Stats = ({ gameState }) => {
  return (
    <div className='Stats_playerContainer'>
      {gameState && (
        <>
          {gameState.players.map((player) => (
            <div className='Stats_player' key={player.name}>
              <h2 className='Stats_stat Stats_playerName'>{player.name}</h2>
              <h2 className='Stats_stat'>
                <AiOutlineHeart />{' '}
                <div className='Stats_counterContainer'>
                  {renderCounters('health', player.health.points, 'red')}
                </div>
              </h2>
              <h2 className='Stats_stat'>
                <GiAmmoBox />
                <div className='Stats_counterContainer'>
                  {renderCounters('ammo', player.ammo, 'yellow')}
                </div>
              </h2>
              <h2 className='Stats_stat'>
                <GiMetalBoot />
                <div className='Stats_counterContainer'>
                  {renderCounters('speed', player.speed, 'blue')}
                </div>
              </h2>
              <h2 className='Stats_stat'>
                <GiWoodenCrate />
                <div className='Stats_counterContainer'>
                  {renderCounters('materials', player.materials, 'brown')}
                </div>
              </h2>
            </div>
          ))}
        </>
      )}
    </div>
  );
};
