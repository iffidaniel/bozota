import React from 'react';
import './GameMap.css';
import {
  BiChevronDownSquare,
  BiChevronUpSquare,
  BiCaretRight,
  BiError
} from 'react-icons/bi';
import {
  FaHeartbeat,
  FaBomb,
  FaBattleNet,
FaBahai
} from 'react-icons/fa';
import {
  BsSuitHeartFill,
  
  
} from 'react-icons/bs';
import {
  AiOutlineBuild,
  AiOutlineFire,
  AiTwotoneGold
  
} from 'react-icons/ai';
import {
  GiHeavyBullets,
  GiBrickWall,
  GiClusterBomb,
  GiBulletImpacts,
  GiClayBrick,
  GiMineExplosion,
} from 'react-icons/gi';

import {GoPrimitiveDot} from 'react/icons/go'

const renderItem = (id, key) => {
  if (id === 0) {
    return <span className='GameMap_cell_empty GameMap_cell' key={key} />;
  } else if (id === 1) {
    return <BsSuitHeartFill className='GameMap_cell_health GameMap_cell' key={key} />;
  } else if (id === 2) {
    return <GiHeavyBullets className='GameMap_cell_ammo GameMap_cell' key={key} />;
  } else if (id === 3) {
    return <GiBrickWall className='GameMap_cell_wall GameMap_cell' key={key} />;
  } else if (id === 4) {
    return <FaBomb className='GameMap_cell_bomb GameMap_cell' key={key} />;
  } else if (id === 5) {
    return <span className='GameMap_cell_player GameMap_cell' key={key} />;
  } else if (id === 6) {
    return <GoPrimitiveDot  className='GameMap_cell_bullet GameMap_cell' key={key} />;
  } else if (id === 7) {
    return <GiMineExplosion className='GameMap_cell_fire GameMap_cell' key={key} />;
  } else if (id === 8) {
    return <AiOutlineBuild className='GameMap_cell_materials GameMap_cell' key={key} />;
  } else {
    return <BiError className='GameMap_cell_error GameMap_cell' key={key} />;
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
