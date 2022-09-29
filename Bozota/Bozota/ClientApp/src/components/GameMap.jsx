import React from "react";
import "./GameMap.css";
import { BiError } from "react-icons/bi";
import { FaBomb, FaRobot, FaTools } from "react-icons/fa";
import { BsSuitHeartFill } from "react-icons/bs";
import { GiHeavyBullets, GiBrickWall, GiMineExplosion } from "react-icons/gi";
import { GoPrimitiveDot } from "react-icons/go";

const renderItem = (id, key, name) => {
  if (id === 0) {
    return <span className="GameMap_cell_empty GameMap_cell" key={key} />;
  } else if (id === 1) {
    return (
      <BsSuitHeartFill className="GameMap_cell_health GameMap_cell" key={key} />
    );
  } else if (id === 2) {
    return (
      <GiHeavyBullets className="GameMap_cell_ammo GameMap_cell" key={key} />
    );
  } else if (id === 3) {
    return <GiBrickWall className="GameMap_cell_wall GameMap_cell" key={key} />;
  } else if (id === 4) {
    return <FaBomb className="GameMap_cell_bomb GameMap_cell" key={key} />;
  } else if (id === 5 || id >= 10) {
    return renderPlayer(id, key);
  } else if (id === 6) {
    return (
      <GoPrimitiveDot
        className={`GameMap_cell ${name}_icon GameMap_cell_empty`}
        key={key}
      />
    );
  } else if (id === 7) {
    return (
      <GiMineExplosion className="GameMap_cell_fire GameMap_cell" key={key} />
    );
  } else if (id === 8) {
    return (
      <FaTools className="GameMap_cell_materials GameMap_cell" key={key} />
    );
  } else {
    return <BiError className="GameMap_cell_error GameMap_cell" key={key} />;
  }
};

const renderPlayer = (id, key) => {
  if (id === 10) {
    return (
      <FaRobot className="Player_icon Daniel_icon GameMap_cell" key={key} />
    );
  } else if (id === 11) {
    return (
      <FaRobot className="Player_icon Veikko_icon GameMap_cell" key={key} />
    );
  } else if (id === 12) {
    return (
      <FaRobot className="Player_icon Krishna_icon GameMap_cell" key={key} />
    );
  } else if (id === 13) {
    return <FaRobot className="Player_icon Raif_icon GameMap_cell" key={key} />;
  } else if (id === 14) {
    return (
      <FaRobot className="Player_icon Ramesh_icon GameMap_cell" key={key} />
    );
  } else if (id === 15) {
    return <FaRobot className="Player_icon Riku_icon GameMap_cell" key={key} />;
  } else {
    return (
      <FaRobot
        className="Player_icon GameMap_cell_player GameMap_cell"
        key={key}
      />
    );
  }
};

const isBullet = (id, ri, ci, bullets) => {
  if (id !== 6) {
    return;
  }

  var correctBullet = bullets.find((b) => b.yPos == ri && b.xPos == ci);
  return correctBullet.playerName;
};

export const GameMap = ({ gameState }) => {
  return (
    <div className="GameMap_outer">
      {gameState && (
        <>
          <div className="GameMap_inner">
            {gameState.map.map((row, ri) => {
              return (
                <div className="row" key={ri}>
                  {row.map((column, ci) => {
                    var name = isBullet(column, ri, ci, gameState.bullets);

                    return renderItem(column, ci, name);
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
