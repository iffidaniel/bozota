import { observable } from 'mobx';
import { updateGame } from '../api/gameControls';

class game {
  state = observable({
    map: null,
    players: null,
  });

  update = () => {
      updateGame().then((response) => {
      this.state = response;
    });
  };
}

export default game;
