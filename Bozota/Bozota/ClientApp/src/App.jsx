import React, { Component } from 'react';
import { GameMap } from './components/GameMap';
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
        <GameMap game={new game()} />
        <Stats />
      </div>
    );
  }
}
