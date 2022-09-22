import React from 'react';
import './App.css';
import Game from './state/game';
import Controls from './state/controls';
import { MainView } from './components/MainView';

export const App = () => {
  const game = new Game();
  const controls = new Controls();

  return (
    <div className='mainPage'>
      <MainView controls={controls} game={game} />
    </div>
  );
};

export default App;
