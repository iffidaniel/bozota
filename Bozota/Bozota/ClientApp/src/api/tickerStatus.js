const fetchTickerStatus = async () => {
  var res = await fetch('game/ticker/status');

  return await res.json();
};

export default fetchTickerStatus;
