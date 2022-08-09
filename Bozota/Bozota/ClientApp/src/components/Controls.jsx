import React, { useEffect, useState } from 'react';
import { initGame, updateGame } from '../api/gameControls';

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
      </div>
    </div>
  );
};
