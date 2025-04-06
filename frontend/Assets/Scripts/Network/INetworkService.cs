namespace Networking
{
    public interface INetworkService
    {
        void Connect();
        void SendPlayerID(string playerID);
        void SendPosition(float x, float y, float z);
        void SendChatMessage(string message);
        void Disconnect();
    }
}
