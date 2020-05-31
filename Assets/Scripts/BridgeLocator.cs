using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;



public class BridgeLocator : MonoBehaviour {
 


    IPAddress multicastAddress = IPAddress.Parse("239.255.255.250");
    const int multicastPort = 1900;
    const int unicastPort = 1901;
    string discover;
    private List<string> _discoveredDevices = new List<string>();

	void Awake () {
        discover = "M-SEARCH * HTTP/1.1\r\n";
        discover += "HOST: 239.255.255.250:1900\r\n";
        discover += "MAN: \"ssdp:discover\"\r\n";
        discover += "MX: 8\r\n";
        discover += "ST:SsdpSearch:all\r\n";
        discover += "\r\n";

 	}


    public void LocateBridge(Action<string> callback = null)
    {
        StartCoroutine(LocateCoroutine(callback));
    }

    private IEnumerator LocateCoroutine(Action<string> callback = null)
    {
        yield return null;
        //below code found on internet and was not done for Unity.
        //I will rewite it later as it will freeze the UI while running

        byte[] broadcastMessage = Encoding.UTF8.GetBytes(discover);
 
        _discoveredDevices = new List<string>();


        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Bind(new IPEndPoint(IPAddress.Any, unicastPort));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddress, IPAddress.Any));
            var thd = new Thread(() => GetSocketResponse(socket));
            socket.SendTo(broadcastMessage, 0, broadcastMessage.Length, SocketFlags.None, new IPEndPoint(multicastAddress, multicastPort));
            thd.Start();
            Thread.Sleep(10000); //10 secondes
            socket.Close();
        }
        //There will be a lof answers. I will not remove the duplicates but send the first following the format  :
        //Device : http://<IP>:80/description.xml
        foreach (string device in _discoveredDevices){
            if (device.StartsWith("http://",StringComparison.InvariantCulture) && device.EndsWith(":80/description.xml", StringComparison.InvariantCulture))
            {
                if (callback != null)
                {
                    callback(device);
                    break;
                }
               
            }
            Debug.Log("Device : " + device);
        }
       
    }
    public void GetSocketResponse(Socket socket)
    {
        try
        {
            while (true)
            {
                var response = new byte[8000];
                EndPoint ep = new IPEndPoint(IPAddress.Any, multicastPort);
                socket.ReceiveFrom(response, ref ep);

                try
                {
                    var receivedString = Encoding.UTF8.GetString(response);

                    var location = receivedString.Substring(receivedString.IndexOf("LOCATION:", System.StringComparison.Ordinal) + 9);
                    receivedString = location.Substring(0, location.IndexOf("\r", System.StringComparison.Ordinal)).Trim();

                    _discoveredDevices.Add(receivedString);
                }
                catch
                {
                    //Not a UTF8 string, ignore this response.
                }
            }
        }
        catch
        {
            //TODO handle exception for when connection closes
        }


    }

}
