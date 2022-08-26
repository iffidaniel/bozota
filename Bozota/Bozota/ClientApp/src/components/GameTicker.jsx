import React, { useEffect, useState } from 'react';
import { GameMap } from './GameMap';

export const GameTicker = (props) => {
    const [mapArray, setMapArray] = useState(null);

    const {
        game
    } = props;


    const updateGameFunction = async () => {
        await game.update();
        setMapArray(game.state.map);
    }
    useEffect(() => {
        const handle = setInterval(() => {
            updateGameFunction();
        }, 400);
        return () => {
            clearInterval(handle);
        }
    }, [mapArray])

   
    return (
        <GameMap mapArray={mapArray}/>
    )
};