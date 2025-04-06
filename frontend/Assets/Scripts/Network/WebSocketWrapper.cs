using System;
using WebSocketSharp;

namespace Networking
{
    public class WebSocketWrapper : IWebSocket
    {
        private WebSocket ws;

        public WebSocketWrapper(string url)
        {
            ws = new WebSocket(url);
        }

        public event EventHandler<MessageEventArgs> OnMessage
        {
            add { ws.OnMessage += value; }
            remove { ws.OnMessage -= value; }
        }

        public void Connect() => ws.Connect();
        public void Send(string message) => ws.Send(message);
        public void Close() => ws.Close();
        public bool IsOpen => ws.ReadyState == WebSocketState.Open;
    }
}
