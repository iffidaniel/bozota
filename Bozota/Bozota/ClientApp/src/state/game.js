import { observable, makeAutoObservable } from 'mobx';
import fetchGameStatus from '../api/gameStatus';

class game {
  state = observable({
    xCellCount: 0,
    yCellCount: 0,
    map: null,
    players: null,
  });

  update = () => {
    fetchGameStatus().then((response) => {
      this.state = response;
    });
  };
}

export default game;
