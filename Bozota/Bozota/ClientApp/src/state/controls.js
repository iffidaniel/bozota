import { makeAutoObservable } from 'mobx';

class Controls {
  state = {
    started: false,
    stopped: false,
    interval: 400,
    maxInterval: 1000,
    minInterval: 100,
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

  increaseSpeed() {
    if (this.state.interval > this.state.minInterval) {
      this.state.interval -= 100;
    }
  }

  decreaseSpeed() {
    if (this.state.interval < this.state.maxInterval) {
      this.state.interval += 100;
    }
  }
}

export default Controls;
