const fetchGameStatus = async () => {
  const res = await fetch('game/status');
  const data = await res.json();

  return data;
};

export default fetchGameStatus;
