import React from 'react';
import * as api from '../api';
import { observer } from 'mobx-react-lite';
import {
  BiChevronDownSquare,
  BiChevronUpSquare,
  BiCaretRight,
} from 'react-icons/bi';
import './ControlPanel.css';

export const ControlPanel = observer(({ controls }) => {
  const renderSpeedGauge = (value) => {
    const counters = [];
    value = controls.state.maxInterval + 100 - value;
    for (var i = 0; i < value; i += 100) {
      counters.push(
        <BiCaretRight
          key={`speed_${i}`}
          className={`ControlPanel_speedCounter`}
        />
      );
    }
    return counters;
  };

  return (
    <div className='ControlPanel_outer'>
      {controls.state.started && (
        <div className='ControlPanel_speed_outer'>
          <span className='ControlPanel_speedTitle'>Speed</span>
          <div className='ControlPanel_speedGauge'>
            {renderSpeedGauge(controls.state.interval)}
          </div>
          <div className='ControlPanel_speed_inner'>
            <button
              className='ControlPanel_speedButton'
              onClick={() => controls.increaseSpeed()}
            >
              <BiChevronUpSquare />
            </button>
            <button
              className='ControlPanel_speedButton'
              onClick={() => controls.decreaseSpeed()}
            >
              <BiChevronDownSquare />
            </button>
          </div>
        </div>
      )}
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
