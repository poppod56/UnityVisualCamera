using System.Net;
using System.Net.Sockets;
using UnityEngine;


namespace Poppod
{
    public class DisplayIPAddress : MonoBehaviour
    {
        private string localIPAddress;

        private void Start()
        {
            localIPAddress = GetLocalIPAddress();
            Debug.Log("Local IP Address: " + localIPAddress);
        }

        // Function to get the local IP address
        private string GetLocalIPAddress()
        {
            string ipAddress = "Unknown IP Address";

            try
            {
                // Get host name
                string hostName = Dns.GetHostName();

                // Get the IP addresses associated with the host
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);

                foreach (IPAddress address in addresses)
                {
                    // Check for IPv4 address
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = address.ToString();
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                Debug.LogError("SocketException: " + ex.Message);
            }

            return ipAddress;
        }

        private void OnGUI()
        {
            // Display the local IP address in a simple UI
            GUILayout.Label("Local IP Address: " + localIPAddress);
        }
    }
}

