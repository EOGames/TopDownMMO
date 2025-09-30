const express = require("express");
const http = require("http");
const { Server } = require("socket.io");
const { login } = require("./controllers/controller");

const app = express();
const server = http.createServer(app);
const io = new Server(server, {
    cors: {
        origin: "*", // allow Unity client
        methods: ["GET", "POST"]
    }
});

const alreadySpawnedPlayers = {};

// To parse JSON bodies
app.use(express.json());

// To parse application/x-www-form-urlencoded (Unity WWWForm sends this)
app.use(express.urlencoded({ extended: true }));


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
            socket.emit("spawnPlayer", data);
            io.emit('existingPlayers', alreadySpawnedPlayers);
            console.log("Brodcast exisiting players !");
        } catch (error) {
            console.log(`❌ Error while request Player Spawn Error: ${error}`);
        }
    });

    socket.on("movement", (resp) => {
        try {
            let data;
            if (typeof resp == "string") {
                data = JSON.parse(resp);
                io.emit("movement", data);
                // console.log("Movement Data Sent to clones");
            }
            // console.log(`✅ Request For Movement Received ::: ${resp}`);
        } catch (error) {
            console.log(`❌ Error while request Movement Error: ${error}`);
        }
    });

    socket.on("shootWeapon", (resp) => {
        console.log(" 🎯 Shoot Event Received");
        try {
            let data;
            if (typeof resp == 'string') {
                data = JSON.parse(resp);
            }
            console.log("Data For Shoot Event : ", data);
            io.emit("shootWeapon", data);
            console.log(" 🟢 Weapon Shoot Data Sent To Clones !");
        } catch (error) {
            console.log(` Error While Sending Shoot Event Error: ${error}`);
        }
    });


    socket.on("disconnect", () => {
        try {
            console.log("❌ A player disconnected:", socket.id);
            console.log("❌ And disconnected Player Id Is:", alreadySpawnedPlayers[socket.id].id);
            io.emit("playerDisconnected", alreadySpawnedPlayers[socket.id].id)
            delete alreadySpawnedPlayers[socket.id];
        } catch (error) {
            console.log(` Error Discconecting Player and Error is ${error}`);
        }
    });
});

//URL path
app.get('/', (req, res) => {
    res.send("server Connected 😊🙏");
});

app.post('/login', login);

server.listen(3000, () => {
    console.log("🚀 Server running on http://localhost:3000");
});
