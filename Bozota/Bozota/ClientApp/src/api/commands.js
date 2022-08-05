export const initGame = async () => {
  const res = await fetch('game/init');
};

export const startGame = async () => {
  await fetch('game/ticker/start');
};

export const stopGame = async () => {
  await fetch('game/ticker/stop');
};
