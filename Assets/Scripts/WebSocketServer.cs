using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class WebSocketServer : MonoBehaviour
{
    private HttpListener httpListener;
    private WebSocket webSocket;

    public CameraController cameraController; // Assign this in the Unity Inspector
    public bool isTransformEnabled = false;   // Flag to control transform application manually

    private void Start()
    {
        StartServer();
    }


    // Start the WebSocket server
    public void StartServer()
    {
        // Initialize the HttpListener and set the listening address and port
        httpListener = new HttpListener();
        //httpListener.Prefixes.Add("http://localhost:8080/"); // You can change the port if needed
        httpListener.Prefixes.Add("http://*:8080/");
        httpListener.Start();

        Debug.Log("WebSocket server started on ws://localhost:8080/");

        // Start listening for WebSocket connections
        Task.Run(() => ListenForConnections());
    }

    // Asynchronously listen for incoming connections
    private async void ListenForConnections()
    {
        while (httpListener.IsListening)
        {
            // Wait for a WebSocket connection
            var context = await httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                // Accept the WebSocket connection
                var webSocketContext = await context.AcceptWebSocketAsync(null);
                webSocket = webSocketContext.WebSocket;

                Debug.Log("WebSocket client connected!");

                // Start receiving data from the WebSocket client
                ListenForMessages();
            }
        }
    }

    // Listen for incoming messages from the WebSocket client
    private async void ListenForMessages()
    {
        byte[] buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            try
            {
                // Receive a message from the client
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                // If the message type is text, process the transform data
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.Log($"Received: {receivedMessage}");

                    // Process the transform data (parse JSON and apply it)
                    ProcessTransformData(receivedMessage);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("WebSocket error: " + e.Message);
                break;
            }
        }

        Debug.Log("WebSocket client disconnected.");
    }

    // Parse and apply the transform data to the camera in the Unity Editor
    private void ProcessTransformData(string message)
    {
        try
        {
            // Parse the JSON message into a TransformData object
            TransformData transformData = JsonUtility.FromJson<TransformData>(message);

            // Check for a valid TransformData object
            if (transformData != null)
            {
                // Update the camera controller's target transform data
                cameraController.SetTargetTransform(transformData.position, transformData.rotation);


            }
            else
            {
                Debug.LogWarning("Failed to parse transform data from message: " + message);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error processing transform data: " + ex.Message);
        }
    }

    // Call this method to start applying the transform when connected
    public void EnableTransform()
    {
        isTransformEnabled = true;
    }

    // Call this method to stop applying the transform
    public void DisableTransform()
    {
        isTransformEnabled = false;
    }

    // Clean up the WebSocket and HttpListener on destroy
    private void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server shutting down", CancellationToken.None);
        }

        httpListener?.Stop();
    }
}


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