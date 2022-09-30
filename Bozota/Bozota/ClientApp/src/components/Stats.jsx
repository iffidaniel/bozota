import React from "react";
import "./Stats.css";
import { BsSuitHeartFill } from "react-icons/bs";
import { GiAmmoBox, GiHeavyBullets, GiWoodenCrate } from "react-icons/gi";
import { FaBomb, FaRobot, FaTools } from "react-icons/fa";

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
    <div className="Stats_playerContainer">
      {gameState && (
        <>
          {gameState.players.map((player) => (
            <div className="Stats_player" key={player.name}>
              <h2 className={`Stats_stat Stats_playerName ${player.name}`}>
                {player.name}
              </h2>
              <h2 className="Stats_stat">
                <BsSuitHeartFill />{" "}
                <div className="Stats_counterContainer">
                  {renderCounters("health", player.health.points, "red")}
                </div>
              </h2>
              <h2 className="Stats_stat">
                <GiHeavyBullets />
                <div className="Stats_counterContainer">
                  {renderCounters("ammo", player.ammo, "yellow")}
                </div>
              </h2>
              {/* <h2 className='Stats_stat'>
                <GiMetalBoot />
                <div className='Stats_counterContainer'>
                  {renderCounters('speed', player.speed, 'blue')}
                </div>
              </h2> */}
              <h2 className="Stats_stat">
                <FaTools />
                <div className="Stats_counterContainer">
                  {renderCounters("materials", player.materials, "brown")}
                </div>
              </h2>
            </div>
          ))}
        </>
      )}
    </div>
  );
};
