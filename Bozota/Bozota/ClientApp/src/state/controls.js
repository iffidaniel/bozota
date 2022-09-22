import { makeAutoObservable } from 'mobx';

class Controls {
  state = {
    stopped: false,
    speed: 400,
  };

  constructor() {
    makeAutoObservable(this);
  }

  toggleStopGame() {
    this.state.stopped = !this.state.stopped;
  }
}

export default Controls;
