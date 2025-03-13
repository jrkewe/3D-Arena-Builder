const express = require('express');
const WebSocket = require('ws');

const app = express();
const server = require('http').createServer(app);
const wss = new WebSocket.Server({ server });

app.use(express.json());

// REST API (testowe)
app.get('/', (req, res) => res.send('Server Running'));

// WebSocket - obsluga polaczen
wss.on('connection', ws => {
    console.log('Client connected');
    ws.on('message', message => {
		console.log('Received:', message);
		ws.send('Echo: ${message}');
	});
    ws.send('Connected to WebSocket Server');
});

// Uruchomienie serwera
server.listen(3000, () => console.log('Server running on port 3000'));