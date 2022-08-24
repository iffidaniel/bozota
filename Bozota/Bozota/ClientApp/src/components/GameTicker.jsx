import React, { useState } from 'react';
import { GameMap } from './GameMap';

export const GameTicker = (props) => {
  const [mapArray, setMapArray] = useState(null);
    
    const {
        game      
  } = props;    

    setInterval(() => {
        game.update();
      //  setMapArray(game.state.map);
    }, 1000);

    return (
        <GameMap mapArray={mapArray}/>  
)        
};