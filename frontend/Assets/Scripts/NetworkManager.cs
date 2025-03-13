using UnityEngine;
using WebSocketSharp;

public class NetworkManager : MonoBehaviour
{
    private WebSocket ws;

    void Start()
    {
        ws = new WebSocket("ws://localhost:3000");

        ws.OnMessage += (sender, e) => Debug.Log("Server Response: " + e.Data);

        ws.Connect();
        ws.Send("Hello from Unity!");
    }

    void OnDestroy()
    {
        ws.Close();
    }
}