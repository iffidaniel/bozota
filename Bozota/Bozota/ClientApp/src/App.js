import React, { Component } from 'react';
import { BattleMap } from './components/BattleMap';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return <BattleMap />;
  }
}
