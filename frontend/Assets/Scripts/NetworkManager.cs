using UnityEngine;
using WebSocketSharp;

public class NetworkManager : MonoBehaviour
{
    private WebSocket ws;
    private string playerID;

    void Start()
    {
        playerID = PlayerPrefs.GetString("playerID", System.Guid.NewGuid().ToString());
        PlayerPrefs.SetString("playerID", playerID);
        PlayerPrefs.Save();

        ws = new WebSocket("ws://localhost:3000");

        ws.OnMessage += (sender, e) => Debug.Log("Server Response: " + e.Data);

        ws.Connect();

        SendID();
        SendMove(10.0f,5.0f,2.0f);
        SendChat("Hello, server!");
    }

    //Send id
    void SendID()
    {
        if (ws.ReadyState == WebSocketState.Open) {
            string message = $"{{\"type\": \"connect\", \"playerID\":\"{playerID}\"}}";
            Debug.Log($"Gracz ID przed wyslaniem: {playerID}");
            ws.Send(message);
            Debug.Log($"Wyslano ID gracza: {playerID}");
        }
    }

    //Send position
    void SendMove (float x, float y, float z){
        string message = $"{{\"type\": \"move\", \"x\":{x}, \"y\":{y}, \"z\":{z} }}";
        ws.Send(message);
    }

    //Send chat
    void SendChat(string text) { 
        string message = $"{{\"type\":\"chat\", \"text\":\"{text}\"}}";
        ws.Send(message);
    }

    void OnDestroy()
    {
        ws.Close();
    }
}