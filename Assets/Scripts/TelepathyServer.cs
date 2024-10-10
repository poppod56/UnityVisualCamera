using System;
using System.Text;
using Telepathy;
using UnityEngine;

public class TelepathyServer : MonoBehaviour
{
    public Transform serverCameraTransform;  // Reference to the normal camera in Scene B
    private Telepathy.Server server;

    void Start()
    {
        // Create server and hook up events
        server = new Telepathy.Server(1024);
        
        // Handle when a client connects
        server.OnConnected = (connectionId,msg) =>
        {
            Debug.Log(connectionId + " Connected to the server.");
        };

        // Handle when data is received from a client
        server.OnData = (connectionId, message) =>
        {
            // Convert the received byte array to a JSON string and parse it
            string receivedData = Encoding.UTF8.GetString(message.Array, message.Offset, message.Count);
            TransformData transformData = JsonUtility.FromJson<TransformData>(receivedData);

            // Apply the transform data to the server camera
            serverCameraTransform.position = transformData.position;
            serverCameraTransform.rotation = transformData.rotation;

            Debug.Log("Received transform data from client");
        };

        // Handle when a client disconnects
        server.OnDisconnected = (connectionId) =>
        {
            Debug.Log(connectionId + " Disconnected from the server.");
        };

        // Start the server on port 8080
        server.Start(8080);
        Debug.Log("Server started on port 8080");
    }

    void Update()
    {
        // Tick to process incoming messages
        server.Tick(100);  // You should call this regularly to process messages
    }

    void OnApplicationQuit()
    {
        // Stop the server when the application quits
        server.Stop();
    }
}


// Class to represent the transform data (position and rotation)
[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;

    public TransformData(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
