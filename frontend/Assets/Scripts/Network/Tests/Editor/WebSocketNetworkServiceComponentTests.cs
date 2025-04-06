using NUnit.Framework;
using UnityEngine;
using System.Reflection;
using Networking;
using System;
using WebSocketSharp;

public class WebSocketNetworkServiceComponentTests
{
    private GameObject gameObject;
    private WebSocketNetworkServiceComponent networkService;

    // Mock implementation
    public class MockWebSocket : IWebSocket
    {
        public string LastSentMessage { get; private set; }

        public void Send(string message)
        {
            LastSentMessage = message;
        }

        public void Connect()
        {
            // Mocked connect logic (can be empty)
        }

        public void Close()
        {
            // Mocked close logic (can be empty)
        }

        public bool IsOpen => true;

        public event EventHandler<MessageEventArgs> OnMessage;
    }

    [SetUp]
    public void SetUp()
    {
        gameObject = new GameObject();
        networkService = gameObject.AddComponent<WebSocketNetworkServiceComponent>();

        var mockWS = new MockWebSocket();
        var wsField = typeof(WebSocketNetworkServiceComponent)
            .GetField("ws", BindingFlags.NonPublic | BindingFlags.Instance);
        wsField?.SetValue(networkService, mockWS);
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(gameObject);

    }

    [Test]
    public void Component_IsNotNull()
    {
        Assert.IsNotNull(networkService);
    }

    [Test]
    public void SendPlayerID_SetsLastPlayerIDCorrectly()
    {
        string testID = "test-123";
        networkService.SendPlayerID(testID);

        Assert.AreEqual(testID, networkService.GetLastPlayerID());
    }
}
