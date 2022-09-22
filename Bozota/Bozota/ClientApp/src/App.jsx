import React, { useState, useEffect } from 'react';
import { Stats } from './components/Stats';
import { Controls } from './components/Controls';
import './App.css';
import Game from './state/game';
import { GameMap } from './components/GameMap';

export const App = () => {
  const game = new Game();

  const [gameState, setGameState] = useState(null);

  const updateGameState = async () => {
    await game.update();
    setGameState(game.state);
  };

  useEffect(() => {
    const handle = setInterval(() => {
      updateGameState();
    }, 400);

    return () => {
      clearInterval(handle);
    };
  }, [gameState]);

  return (
    <div className='mainPage'>
      <Controls />
      <GameMap gameState={gameState} />
      <Stats />
    </div>
  );
};

export default App;
