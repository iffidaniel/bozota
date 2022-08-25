import React, { Component } from 'react';
import { GameTicker } from './components/GameTicker';
import { Stats } from './components/Stats';
import { Controls } from './components/Controls';
import './App.css';
import  Game  from './state/game';

export const App = () =>  {
    const displayName = App.name;
    const game = new Game();

 
    return (
      <div className='mainPage'>
        <Controls />
        <GameTicker game={game} />
        <Stats />
      </div>
    );
 
}

export default App;