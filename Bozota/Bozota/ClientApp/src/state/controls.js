import { makeAutoObservable } from 'mobx';

class Controls {
  state = {
    started: false,
    stopped: false,
    speed: 400,
  };

  constructor() {
    makeAutoObservable(this);
  }

  toggleStopGame() {
    this.state.stopped = !this.state.stopped;
  }

  toggleStartGame() {
    this.state.started = !this.state.started;
  }
}

export default Controls;
