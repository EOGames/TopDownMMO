module.exports.GenrateId = (prefix, postfix) => {
    const curTime = Date.now();
    const randomNumber = Math.random() * 1000;
    const anotherRandom = Math.random() * 2000;
    const id = `Id_${prefix}_${curTime}_${randomNumber}_${anotherRandom}_${postfix}`;
    return id;
}