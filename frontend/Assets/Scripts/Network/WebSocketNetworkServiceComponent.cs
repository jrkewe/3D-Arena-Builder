using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;

namespace Networking
{
    public class WebSocketNetworkServiceComponent : MonoBehaviour, INetworkService
    {
        private IWebSocket ws;
        private string serverUrl = "ws://localhost:3000";
        private string lastPlayerID;

        public void Connect()
        {
            ws = new WebSocketWrapper(serverUrl);
            ws.OnMessage += (sender, e) =>
            {
                Debug.Log("Server Response: " + e.Data);
            };

            ws.Connect();
            Debug.Log("Connected to server.");
        }

        public void SendPlayerID(string playerID)
        {
            lastPlayerID = playerID;
            string message = $"{{\"type\": \"connect\", \"playerID\":\"{playerID}\"}}";
            ws.Send(message);
        }

        public void SendPosition(float x, float y, float z)
        {
            if (ws != null && ws.IsOpen)
            {
                string message = $"{{\"type\": \"move\", \"x\":{x}, \"y\":{y}, \"z\":{z} }}";
                ws.Send(message);
            }
        }

        public void SendChatMessage(string message)
        {
            if (ws != null && ws.IsOpen)
            {
                string json = $"{{\"type\":\"chat\", \"text\":\"{message}\"}}";
                ws.Send(json);
            }
        }

        public void Disconnect()
        {
            if (ws != null && ws.IsOpen)
            {
                ws.Close();
                Debug.Log("Disconnected from server.");
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        public string GetLastPlayerID() => lastPlayerID;

        // For testing
        public void SetWebSocket(IWebSocket socket)
        {
            ws = socket;
        }
    }
}
