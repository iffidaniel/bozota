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
    console.log('stop');
    this.state.stopped = !this.state.stopped;
  }
}

export default Controls;
