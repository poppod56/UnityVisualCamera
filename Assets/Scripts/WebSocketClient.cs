using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket webSocket;
    [SerializeField]
    string _url = "ws://192.168.1.103:8080/";
    //private Uri serverUri = new Uri("ws://localhost:8080/");
    private bool isConnected = false; // Flag to track connection status

    private void Start()
    {
        webSocket = new ClientWebSocket();
        ConnectToServer();
    }

    private async void ConnectToServer()
    {
        try
        {
            Uri serverUri = new Uri(_url);
            await webSocket.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("Connected to WebSocket server!");
            isConnected = true;
        }
        catch (Exception ex)
        {
            Debug.LogError("WebSocket connection error: " + ex.Message);
        }
    }

    private async void LateUpdate()
    {
        // Only send transform data if WebSocket is connected
        if (isConnected && webSocket.State == WebSocketState.Open)
        {
            // Create a TransformData object from the current AR camera transform
            TransformData transformData = new TransformData(transform.position, transform.rotation);

            // Convert to JSON
            string jsonData = JsonUtility.ToJson(transformData);
            byte[] messageBytes = Encoding.UTF8.GetBytes(jsonData);

            // Send data to server
            try
            {
                await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error sending transform data: " + ex.Message);
                isConnected = false;
            }
        }
    }

    private void OnDestroy()
    {
        if (webSocket != null)
        {
            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client shutting down", CancellationToken.None);
        }
    }
}
