const express = require("express");
const http = require("http");
const { Server } = require("socket.io");
const { GenrateId } = require("./tools/tools");

const app = express();
const server = http.createServer(app);
const io = new Server(server, {
    cors: {
        origin: "*", // allow Unity client
        methods: ["GET", "POST"]
    }
});

const alreadySpawnedPlayers = {};

io.on("connection", (socket) => {
    console.log("✅ A player connected:", socket.id);


    socket.on("requestPlayerSpawn", (resp) => {
        try {
            let data;
            if (typeof resp === "string") {
                data = JSON.parse(resp);
            }
            console.log(`✅ Request To Spawn Player Received ::: ${resp} ${data.characterName}`);
            alreadySpawnedPlayers[socket.id] = data;
            data.socketId = socket.id;
            data.id = GenrateId("Player",data.characterName);
            socket.emit("spawnPlayer", data);
           io.emit('existingPlayers', alreadySpawnedPlayers);
            console.log("Brodcast exisiting players !");
        } catch (error) {
            console.log(`❌ Error while request Player Spawn Error: ${error}`);
        }
    });


    socket.on("disconnect", () => {
        console.log("❌ A player disconnected:", socket.id);
delete alreadySpawnedPlayers[socket.id];
    });
});

app.get('/', (req, res) => {
    res.send("server Connected 😊🙏");
});
server.listen(3000, () => {
    console.log("🚀 Server running on http://localhost:3000");
});
