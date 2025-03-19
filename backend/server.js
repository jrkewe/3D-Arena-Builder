const express = require('express');
const WebSocket = require('ws');
const {v4:uuidv4} = require('uuid');

const app = express();
const server = require('http').createServer(app);
const wss = new WebSocket.Server({ server });

app.use(express.json());
app.get('/', (req, res) => res.send('WebSocket Server Running'));

let players = {};

// WebSocket - obsluga polaczen
wss.on('connection', (ws) => {
	
	let playerID = null;
	
    ws.on('message',(message)=> {
		console.log("Otrzymano wiadomosc:", message.toString());
		
		try{
			const data = JSON.parse(message);
			console.log("Sparsowane dane:", data);
			
			if(data.type ==="connect"){
				playerID = data.playerID;
				if(!players[playerID]){
					players[playerID] = {id: playerID, x: 0, y:0, z:0};
				}
				console.log(`Gracz ${playerID} polaczony.`);
				ws.send(JSON.stringify({type: "player_data", player: players[playerID]}));
			} else if (data.type ==="move"){
				players[playerID].x = data.x;
				players[playerID].y = data.y;
				players[playerID].z = data.z;
				broadcast({type:"player_moved", player: players[playerID]});
			} else {
				console.log("Unknown message type: ", data.type);
			}
		} catch (error) {
			console.error("Invalid JSON received:", message);
		}
	});
	
	ws.on('close', ()=>{
		console.log(`Gracz ${playerID} rozlaczony.`);
		delete players[playerID];
		broadcast({type:"player_left", playerID});
	});
	
	function broadcast(data){
		wss.clients.forEach(client=>{
			if(client.readyState === WebSocket.OPEN){
				client.send(JSON.stringify(data));
			}
		});
	}
	
});

// Uruchomienie serwera
server.listen(3000, () => console.log('Server running on port 3000'));