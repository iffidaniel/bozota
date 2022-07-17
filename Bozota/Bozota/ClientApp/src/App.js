import React, { Component } from 'react';
import { BattleMap } from './components/BattleMap';
import { Stats } from './components/Stats';
import './App.css';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <div className='mainPage'>
        <BattleMap />
        <Stats />
      </div>
    );
  }
}
