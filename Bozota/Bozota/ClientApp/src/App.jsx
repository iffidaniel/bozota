import React, { Component } from 'react';
import { GameTicker } from './components/GameTicker';
import { Stats } from './components/Stats';
import { Controls } from './components/Controls';
import './App.css';
import game from './state/game';

export default class App extends Component {
    static displayName = App.name;

  render() {
    return (
      <div className='mainPage'>
        <Controls />
        <GameTicker game={new game()} />
        <Stats />
      </div>
    );
  }
}
