using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class NetServer : MonoBehaviour
{
    public static NetServer Instance;

    public NetClient.Scene currentScene;

    public TcpListener server;
    public UdpClient udp;
    public bool serverStarted = false;

    public List<ClientToken> clients;
    public List<ClientToken> disconnectList;
    public List<string[]> transformList;
    public int hostId = -1;

    //netstat -anp tcp/udp | find "LISTEN" 
    //netstat -anp tcp/udp | find "Port" 
    public string localIp;
    public string port;

    public byte[] data = new byte[1024];

    public int nextId = 0;

    public bool transformSending = false;
    public float transformDelay = 0.1f;

    public NetServerEvent netServerEvent;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        netServerEvent = new NetServerEvent();
    }

    private void Start()
    {

        clients = new List<ClientToken>();
        disconnectList = new List<ClientToken>();
        transformList = new List<string[]>();
        currentScene = NetClient.Scene.SocketChatTutorialScene;
        //GetLocalIP();

    }

    internal void DontDestory()
    {
        DontDestroyOnLoad(gameObject);
    }

    public bool GetServerStarted()
    {
        return serverStarted;
    }

    private void OnApplicationQuit()
    {
        CloseServer();
    }

    private void GetLocalIP()
    {
        IPHostEntry host = Dns.GetHostEntry("교사용");
        
       foreach (IPAddress ip in host.AddressList)
        {
            if(ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIp = ip.ToString();

                if (ChatManager.Instance != null)
                {
                    ChatManager.Instance.SetLocalIP(localIp);
                }

                Debug.Log(localIp);
            }
        }

    }

    public void InitializeServer(int port)
    {
        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            StartListening();

            OnServerStarted(port);
        }
        catch (Exception e)
        {
            Debug.Log("Socket Start Error : "+ e);
        }
    }

    private void Update()
    {
        if (!serverStarted) return;

        foreach (ClientToken client in clients)
        {
            if (!IsConnected(client.tcp))
            {
                //TODO: Disconnect 처리
                disconnectList.Add(client);
                continue;
            }

            try
            {
                NetworkStream stream = client.tcp.GetStream();

                if (stream.DataAvailable)
                {
                    int length = stream.Read(data, 0, data.Length);

                    byte[] readData = new byte[length];

                    Array.Copy(data, 0, readData, 0, length);

                    if (readData != null)
                    {
                        NetPacket packet = new NetPacket();
                        netServerEvent.OnEvent(packet, client);
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.Log(e);
            }
        }

        for (int i = 0; i < disconnectList.Count; i++)
        {
            clients.Remove(disconnectList[i]);
            Broadcast(new NetPacket(NetProtocol.SYS_CLIENT_DISCONNECT,disconnectList[i].id));
        }
        if (disconnectList.Count > 0)
        {
            bool hostExit = disconnectList.Exists(token => token.id == hostId);

            disconnectList.Clear();

            // Set New Host
            if (hostExit)
            {
                try
                {
                    SetHost(clients[0]);
                }
                catch
                {
                    hostId = -1;
                }

            }
            
        }
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if(c.Client !=null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// On Server Received Data
    /// </summary>
    /// <param name="packet">Data</param>
    /// <param name="token">Client</param>
    

    // Server -> All Clients
    public void Broadcast(NetPacket packet)
    {
        foreach (ClientToken client in clients)
        {
            SendData(client, packet);
        }

    }

    public void BroadcastExcept(NetPacket packet, int exceptionIndex)
    {
        for(int i = 0; i < clients.Count; i++)
        {
            if(exceptionIndex == i)
            {
                continue;
            }
            SendData(clients[i], packet);
        }

    }

    public IEnumerator SendTransformCoroutine()
    {
        while(transformSending)
        {
            NetPacket packet = new NetPacket(NetProtocol.RES_PLAYER_TRANSFORM, transformList);

            Broadcast(packet);

            yield return new WaitForSeconds(transformDelay);
        }
    }

    public void SetHost(ClientToken token)
    {
        hostId = token.id;
        SendData(token, new NetPacket(NetProtocol.SYS_SET_HOST));
    }
    
    // Send To Target Client
    private void SendData(ClientToken client, NetPacket packet)
    {
        try
        {
            NetworkStream stream = client.tcp.GetStream();
            stream.Write(packet.packetData, 0, packet.packetData.Length);
            stream.Flush();
        }
        catch (SocketException e)
        {
            Debug.Log(e);
        }
    }

    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        clients.Add(new ClientToken(listener.EndAcceptTcpClient(ar), "Player " + clients.Count, nextId));
        nextId++;
        //Debug.Log("AcceptClient!");
        StartListening();
    }

    public void CloseServer()
    {
        if (!serverStarted) return;

        clients.Clear();

        //server
        server.Stop();
        server = null;

        ChangeStatus(false);
    }

    private void OnServerStarted(int port)
    {
        this.port = port.ToString();

        EndPoint ep = server.LocalEndpoint;
        //Debug.Log(ep);
        ChangeStatus(true);
    }
    
    private void ChangeStatus(bool open)
    {
        serverStarted = open;

        //int num = (int)Scene;
        //string numStr = string.Format("{0:00}", num);
        var a = Enum.GetNames(typeof(NetClient.Scene));
        foreach(var item in a)
        {
            
        }

        // 99. MultiplayScene
        string cs = SceneManager.GetActiveScene().name;
        string[] sceneNames = cs.Split('.');
        //  MultiplayScene
        sceneNames[1].Substring(1);



        //SceneManager.LoadScene($"{numStr}. {Scene}")

        if (currentScene == NetClient.Scene.SocketChatTutorialScene)
        {
            ChatManager.Instance.OnChangeServerStatus(serverStarted);
        }

        // my PlayerListIndex
        // Start
        // PlayerList[myInd].cinemachineCam의 priority 값 증가
    }

    public void SendClientList(ClientToken token)
    {
        // TODO: Send Client Name List string Array or String List
        string[] names = new string[clients.Count];
        for (int i = 0; i < clients.Count; i++)
        {
            names[i] = clients[i].clientName+"&"+clients[i].id;
        }
        SendData(token, new NetPacket(NetProtocol.SYS_CLIENT_LIST, names));
    }

}

[Serializable]
public class ClientToken
{
    public TcpClient tcp;
    public int id;
    public int listOrder = -1;
    public string clientName;

    public ClientToken(string name,int id, int listOrder)
    {
        clientName = name;
        this.id = id;
        this.listOrder = listOrder;
    }

    public ClientToken(TcpClient tc, string name,int id = -1, int listOrder = -1)
    {
        tcp = tc;
        clientName = name;
        this.id = id;
        this.listOrder = listOrder;
    }


}