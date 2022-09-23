import React from 'react';
import * as api from '../api';
import { observer } from 'mobx-react-lite';
import './ControlPanel.css';

export const ControlPanel = observer(({ controls }) => {
  return (
    <div className='ControlPanel_outer'>
      <div className='ControlPanel_inner'>
        <button className='ControlPanel_button' onClick={() => api.initGame()}>
          Reset
        </button>
        <div className='ControlPanel_buttonSeparator'>|</div>
        <button
          className='ControlPanel_button'
          onClick={() => controls.toggleStopGame()}
        >
          {controls.state.stopped ? <>Start</> : <>Stop</>}
        </button>
      </div>
    </div>
  );
});
