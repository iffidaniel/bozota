export const resetGame = async () => {
  const res = await fetch('game/reset');
  const data = await res.json();

  return data;
};

export const updateGame = async () => {
  const res = await fetch('game/update');
  const data = await res.json();

  return data;
};
