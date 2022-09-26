import React from 'react';
import * as api from '../api';
import { observer } from 'mobx-react-lite';
import './ControlPanel.css';

export const ControlPanel = observer(({ controls }) => {
  return (
    <div className='ControlPanel_outer'>
      <div className='ControlPanel_inner'>
        <button
          className='ControlPanel_button'
          onClick={() => {
            if (controls.state.stopped) {
              controls.toggleStopGame();
            }

            controls.toggleStartGame();

            if (!controls.state.started) {
              api.resetGame();
            }
          }}
        >
          {controls.state.started ? <>Reset</> : <>Start</>}
        </button>
        {controls.state.started && (
          <>
            <div className='ControlPanel_buttonSeparator'>|</div>
            <button
              className='ControlPanel_button'
              onClick={() => controls.toggleStopGame()}
            >
              {controls.state.stopped ? <>Continue</> : <>Stop</>}
            </button>
          </>
        )}
      </div>
    </div>
  );
});
