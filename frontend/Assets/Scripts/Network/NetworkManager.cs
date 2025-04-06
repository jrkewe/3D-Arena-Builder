using System.IO;
using UnityEngine;

namespace Networking
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour networkServiceSource; // Assign WebSocketNetworkService
        private INetworkService networkService;

        private string playerID;
        private static string FilePath => Application.persistentDataPath + "/playerID.json";


        private void Awake()
        {
            networkService = GetComponent<INetworkService>();

            if (networkService == null)
            {
                Debug.LogError("NetworkManager: This GameObject must have a component that implements INetworkService.");
            }

            LoadOrGeneratePlayerID();
        }

        private void Start()
        {
            if (networkService == null) return;

            networkService.Connect();
            networkService.SendPlayerID(playerID);
            networkService.SendPosition(1f, 2f, 3f);
            networkService.SendChatMessage("Hello from client!");
        }

        private void OnDestroy()
        {
            networkService?.Disconnect();
        }

        private void LoadOrGeneratePlayerID()
        {
            if (File.Exists(FilePath))
            {
                playerID = File.ReadAllText(FilePath);
            }
            else
            {
                playerID = System.Guid.NewGuid().ToString();
                File.WriteAllText(FilePath, playerID);
            }

            Debug.Log($"Using Player ID: {playerID}");
        }
    }
}
