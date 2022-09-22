import { makeAutoObservable, observable } from 'mobx';
import { updateGame } from '../api/gameControls';

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
