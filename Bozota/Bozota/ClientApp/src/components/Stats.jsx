import React from 'react';
import './Stats.css';

export const Stats = ({ gameState }) => {
  return (
    <div>
      {gameState && (
        <>
          {gameState.players.map((player) => (
            <div className='Stats_playerContainer' key={player.name}>
              <h2>Name: {player.name}</h2>
              <h2>
                Health: {player.health.points} / {player.health.maxPoints}
              </h2>
              <h2>Ammo: {player.ammo}</h2>
              <h2>Speed: {player.speed}</h2>
              <h2>Materials: {player.materials}</h2>
            </div>
          ))}
        </>
      )}
    </div>
  );
};
