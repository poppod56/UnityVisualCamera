using System;
using System.Text;
using Telepathy;
using UnityEngine;

public class TelepathyClient : MonoBehaviour
{
    public Transform arCameraTransform;  // Reference to the AR camera's transform
    private Telepathy.Client client;
    public string serverIP="192.168.1.183";
    public int serverPort=8080;

    void Start()
    {
        // Initialize the Telepathy client with a message buffer size (1024 bytes in this case)
        client = new Telepathy.Client(1024);

        // Hook up client events
        client.OnConnected = () => Debug.Log("Client Connected to Server");
        client.OnData = (message) =>
        {
            // Handle data received from the server
            Debug.Log("Client Data Received: " + BitConverter.ToString(message.Array, message.Offset, message.Count));
        };
        client.OnDisconnected = () => Debug.Log("Client Disconnected from Server");

        // Connect to the server (replace "localhost" with your server's IP if needed)
        client.Connect(serverIP, serverPort);  // Replace IP and port as necessary
    }

    void Update()
    {
        // Ensure client is connected before sending data
        if (client.Connected)
        {
            // Send the AR camera's transform data to the server
            TransformData transformData = new TransformData(arCameraTransform.position, arCameraTransform.rotation);
            string jsonData = JsonUtility.ToJson(transformData);

            // Convert the JSON string to a byte array and send it to the server
            byte[] messageBytes = Encoding.UTF8.GetBytes(jsonData);
            client.Send(new ArraySegment<byte>(messageBytes));

            // Process any incoming messages
            client.Tick(100);  // Process messages and prevent deadlocks (limit to 100ms)
        }
    }

    void OnApplicationQuit()
    {
        // Disconnect the client when the application is closed
        client.Disconnect();
    }
}

// Helper class to serialize and send position and rotation data
// [System.Serializable]
// public class TransformData
// {
//     public Vector3 position;
//     public Quaternion rotation;

//     public TransformData(Vector3 position, Quaternion rotation)
//     {
//         this.position = position;
//         this.rotation = rotation;
//     }
// }
