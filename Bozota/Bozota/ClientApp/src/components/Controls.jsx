import React, { useEffect, useState } from 'react';
import { initGame, updateGame, stopGame } from '../api/gameControls';

export const Controls = () => {
    const [setStatus] = useState({});

    useEffect(() => {
        updateGame((res) => setStatus(res));
    });

    return (
        <div>
            <div>
                <button onClick={initGame}>Init game</button>
                <button onClick={updateGame}>Update game</button>
                <button onClick={stopGame}>Reset game</button>
            </div>
        </div>
    );
};
