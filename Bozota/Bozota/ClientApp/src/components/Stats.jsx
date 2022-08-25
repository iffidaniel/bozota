import React from 'react';
import './Stats.css';

export const Stats = () => {
  const [stats] = React.useState([
    { name: 'player 1', health: 100, ammo: 5 },
    { name: 'player 2', health: 100, ammo: 5 },
    { name: 'player 3', health: 100, ammo: 5 },
    { name: 'player 4', health: 100, ammo: 5 },
  ]);

  return (
    <div>
      {stats.map((stat) => (
        <div className='playerContainer' key={stat.name}>
          <h2>Name: {stat.name}</h2>
          <h2>Health: {stat.health}</h2>
          <h2>Ammo: {stat.ammo}</h2>
        </div>
      ))}
    </div>
  );
};
