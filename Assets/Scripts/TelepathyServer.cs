using System;
using System.Text;
using Telepathy;
using UnityEngine;

public class TelepathyServer : MonoBehaviour
{
    public Transform parentTransform;         // Reference to the parent of the camera
    public Transform cameraTransform;         // Reference to the camera itself (child of the parent)
    public Vector3 initialLocalPosition;      // Initial local position of the camera relative to the parent
    public Vector3 initialLocalRotation;      // Initial local rotation of the camera relative to the parent
    private Vector3 positionOffset;           // Position offset calculated from the initial position
    private bool isCalibrated = false;        // To ensure calibration happens only once
    private Telepathy.Server server;

    void Start()
    {
        // Ensure that both parent and camera are correctly assigned
        if (parentTransform == null || cameraTransform == null)
        {
            Debug.LogError("Parent or Camera Transform is not assigned in the Inspector!");
            return;
        }

        // Set the initial local position and rotation of the camera relative to the parent
        cameraTransform.localPosition = initialLocalPosition;
        cameraTransform.localRotation = Quaternion.Euler(initialLocalRotation);

        // Create server and hook up events
        server = new Telepathy.Server(1024);

        // Handle when a client connects
        server.OnConnected = (connectionId, msg) =>
        {
            Debug.Log(connectionId + " Connected to the server.");
        };

        // Handle when data is received from a client
        server.OnData = (connectionId, message) =>
        {
            // Convert the received byte array to a JSON string and parse it
            string receivedData = Encoding.UTF8.GetString(message.Array, message.Offset, message.Count);
            TransformData transformData = JsonUtility.FromJson<TransformData>(receivedData);

            // Calibrate the initial position once (first data received)
            if (!isCalibrated)
            {
                // Calculate the position offset (difference between parent initial and client start position)
                positionOffset = parentTransform.position - transformData.position;
                isCalibrated = true;
                Debug.Log("Calibration done. Position Offset: " + positionOffset);
            }

            // Apply the transform data to the parent object (global position/rotation) with calibrated offset
            parentTransform.position = transformData.position + positionOffset;
            parentTransform.rotation = transformData.rotation;

            Debug.Log("Received transform data from client and applied with calibration offset.");
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
