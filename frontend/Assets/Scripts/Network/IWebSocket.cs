using System;
using WebSocketSharp;

namespace Networking
{
    public interface IWebSocket
    {
        event EventHandler<MessageEventArgs> OnMessage;
        void Connect();
        void Send(string message);
        void Close();
        bool IsOpen { get; }
    }
}