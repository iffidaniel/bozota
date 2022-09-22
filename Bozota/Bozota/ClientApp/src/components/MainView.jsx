import React, { useState, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { ControlPanel } from './ControlPanel';
import { GameMap } from './GameMap';
import { Stats } from './Stats';

export const MainView = observer(({ game, controls }) => {
  const [gameState, setGameState] = useState(null);
  const [controlsState, setControlsState] = useState({
    stopped: false,
    speed: 400,
  });

  const updateGameState = async () => {
    await game.update();
    setGameState(game.state);
  };

  useEffect(() => {
    const handle = setInterval(() => {
      if (!controlsState.stopped) {
        updateGameState().then(() => {});
      }
    }, controlsState.speed);

    return () => {
      clearInterval(handle);
    };
  }, [game.state]);

  useEffect(() => {
    setControlsState(controls.state);
  }, [controls.state]);

  return (
    <div>
      <ControlPanel controls={controls} />
      <GameMap gameState={gameState} />
      <Stats />
    </div>
  );
});
