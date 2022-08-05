import React, { Component } from 'react';
import { GameMap } from './components/GameMap';
import { Stats } from './components/Stats';
import { Controls } from './components/Controls';
import './App.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <div className='mainPage'>
        <Controls />
        <GameMap />
        <Stats />
      </div>
    );
  }
}
