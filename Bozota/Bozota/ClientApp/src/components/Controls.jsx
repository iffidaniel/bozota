import React from 'react';
import { initGame, updateGame, stopGame } from '../api';

export const Controls = () => {
  return (
    <div>
      <div>
        <button onClick={initGame}>Init game</button>
        <button onClick={updateGame}>Update game</button>
        <button onClick={stopGame}>Stop game</button>
      </div>
    </div>
  );
};
