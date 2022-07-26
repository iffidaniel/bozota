const fetchGameStatus = async () => {
  var res = await fetch('game/status');

  return await res.json();
};

export default fetchGameStatus;
