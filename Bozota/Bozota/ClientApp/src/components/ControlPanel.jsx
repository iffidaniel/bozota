import React from 'react';
import * as api from '../api';
import { observer } from 'mobx-react-lite';

export const ControlPanel = observer(({ controls }) => {
  return (
    <div>
      <div>
        <button onClick={api.initGame}>Init game</button>
        <button onClick={api.updateGame}>Update game</button>
        <button
          onClick={() => {
            controls.toggleStopGame();
          }}
        >
          Stop game
        </button>
      </div>
    </div>
  );
});
