/// MOCK API

const battleMapArr = () => {
  let rows = [];
  for (let i = 0; i < 100; i++) {
    let columns = [];
    for (let j = 0; j < 100; j++) {
      let rand = Math.floor(Math.random() * 100);
      columns.push(rand);
    }
    rows.push(columns);
  }
  console.log(rows);
  return rows;
};

export default battleMapArr;
