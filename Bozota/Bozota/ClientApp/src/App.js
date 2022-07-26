import React, { Component } from 'react';
import { GameMap } from './components/GameMap';
import { Stats } from './components/Stats';
import './App.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <div className='mainPage'>
        <GameMap />
        <Stats />
      </div>
    );
  }
}
