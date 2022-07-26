import React from 'react';
import './Stats.css';

export const Stats = () => {
  const [stats, setStats] = React.useState([
    { name: 'robo 1', health: 100, ammo: 5, armor: 0 },
    { name: 'robo 2', health: 100, ammo: 5, armor: 0 },
    { name: 'robo 3', health: 100, ammo: 5, armor: 0 },
    { name: 'robo 4', health: 100, ammo: 5, armor: 0 },
  ]);

  return (
    <div>
      {stats.map((stat) => (
        <div className='playerContainer' key={stat.name}>
          <h2>Name: {stat.name}</h2>
          <h2>Health: {stat.health}</h2>
          <h2>Ammo: {stat.ammo}</h2>
          <h2>Armor: {stat.armor}</h2>
        </div>
      ))}
    </div>
  );
};
