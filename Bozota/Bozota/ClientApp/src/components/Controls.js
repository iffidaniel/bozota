import React, { useEffect, useState } from 'react';
import { initGame, startGame, stopGame } from '../api/commands';
import fetchGameStatus from '../api/gameStatus';

export const Controls = () => {
  const [status, setStatus] = useState({});

  useEffect(() => {
    fetchGameStatus((res) => setStatus(res));
  });

  return (
    <div>
      <div>
        <button onClick={initGame}>Init game</button>
        <button onClick={startGame}>Start game</button>
        <button onClick={stopGame}>Stop game</button>
      </div>
      <p>
        Interval: {status.interval}, Status: {status.isRunning}
      </p>
    </div>
  );
};
