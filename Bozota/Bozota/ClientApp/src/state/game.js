import { makeAutoObservable } from 'mobx';
import { updateGame } from '../api';

class Game {
  state;

  constructor() {
    makeAutoObservable(this);
  }

  async update() {
    var res = await updateGame();
    this.state = res;
  }
}

export default Game;
