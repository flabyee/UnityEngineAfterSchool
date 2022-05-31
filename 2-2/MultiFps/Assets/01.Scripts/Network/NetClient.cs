using UnityEngine;
using System.Net.Sockets;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NetClient : MonoBehaviour
{
    public enum Scene
    {
        MultiplayScene = 2,
        MultiplayCoOpScene = 3,
        SocketChatTutorialScene = 99
    }
    public static NetClient Instance;

    private Scene currentScene;

    private ClientToken serverToken;
    private NetworkStream stream;

    private byte[] data = new byte[1024];

    private bool connected = false;

    public List<ClientToken> clients;
    public int myId { get; set; }   
    public int listOrder { get; set; }

    public bool idSet { get; set; }

    public bool isHost { get; set; }

    private NetClientEvent netClientEvent;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        netClientEvent = new NetClientEvent();
    }

    private void Start()
    {
        currentScene = Scene.SocketChatTutorialScene;
        clients = new List<ClientToken>();

        myId = -1;
        listOrder = -1;
    }

    private void Update()
    {
        if (!connected) return;

        if (stream.DataAvailable)
        {
            int length = stream.Read(data, 0, data.Length);
            byte[] readData = new byte[length];

            Array.Copy(data,0,readData,0, length);

            if (readData != null)
            {
                SplitBytesToPackets(readData);
            }
        }
    }

    public bool GetConnected()
    {
        return connected;
    }

    private void OnApplicationQuit()
    {
        DisconnectToServer();
    }

    // readData : [pakcet1][packet2][packet3]..
    private void SplitBytesToPackets(byte[] readData)
    {
        int packetStartPos = 0;

        while (packetStartPos < readData.Length)
        {
            int bodyLength = BitConverter.ToInt32(readData, 4 + packetStartPos);
            int packetLength = bodyLength + 8;
            byte[] packetBytes = new byte[packetLength];
            Array.Copy(readData, packetStartPos, packetBytes, 0, packetLength);

            NetPacket packet = new NetPacket(packetBytes);

            //OnReadData(packet);
            netClientEvent.OnEvent(packet);

            packetStartPos += packetLength;
        }
        
    }

    public ClientToken AddClient(string cn)
    {
        // "Name&Index"
        string[] nameIndexs = cn.Split('&');
        int id = int.Parse(nameIndexs[nameIndexs.Length - 1]);
        string clientName = "";
        for (int j = 0; j < nameIndexs.Length - 1; j++)
        {
            clientName += nameIndexs[j];
        }
        ClientToken token = new ClientToken(clientName, id, clients.Count);

        clients.Add(token);
        return token;
    }

    //private void SceneEvent(NetPacket packet)
    //{
    //    switch(currentScene)
    //    {
    //        case Scene.MultiplayScene:
    //            break;
    //        case Scene.SocketChatTutorialScene:
    //            switch(packet.protocol)
    //            {
                    
    //            }
    //            break;
    //    }
    //}

    //private bool IsChatScene()
    //{
    //    return ChatManager.Instance != null;
    //}

    public void ConnectToServer(string ip, string port, string nickName)
    {

        if (ip == "" || port == "")
        {
            return;
        }
        try
        {
            serverToken = new ClientToken(new TcpClient(ip, int.Parse(port)), nickName);
            //socket = new TcpClient(ip, int.Parse(port));
            stream = serverToken.tcp.GetStream();

            OnConnected();
        }
        catch (Exception e)
        {
            Debug.Log("Client Start Error " + e);
        }

    }

    public void DisconnectToServer()
    {
        if (serverToken == null) return;

        isHost = false;
        serverToken.tcp.Close();
        serverToken = null;

        ChangeStatus(false);

        if (currentScene == Scene.SocketChatTutorialScene)
        {
            ChatManager.Instance.ResetUI();
        }
    }

    // Set Nickname -> Connect To Server -> Send NickName
    private void OnConnected()
    {
        ChangeStatus(true);

        SendNickname();
    }

    private void ChangeStatus(bool connected)
    {
        this.connected = connected;

        if (ChatManager.Instance != null)
        {
            Debug.Log(connected);
            ChatManager.Instance.OnChangeClientStatus(connected);
        }
    }

    #region Send
    public void SendNickname()
    {
        SendData(NetProtocol.REQ_NICKNAME, serverToken.clientName);
    }

    public void SendChat(string msg)
    {
        SendData(NetProtocol.REQ_CHAT, msg);
    }

    // gameType : 0 = MultiPlay, 1 = CoOpPlay
    public void SendStartGame(int gameType)
    {
        SendData(new NetPacket(NetProtocol.REQ_GAME_START, gameType));
    }

    //public void SendTransform(NetTransform trans)
    //{
    //    NetPacket packet = new NetPacket(NetProtocol.REQ_PLAYER_TRANSFORM, trans);

    //    SendData(packet);
    //}

    public void SendTransform(string[] trans)
    {
        NetPacket packet = new NetPacket(NetProtocol.REQ_PLAYER_TRANSFORM, trans);

        //Debug.Log(trans);

        SendData(packet);
    }

    public void SendShoot(string[] shootTrans)
    {
        NetPacket packet = new NetPacket(NetProtocol.EVT_SHOOT_BULLET, shootTrans);

        SendData(packet);
    }

    public void SendData(NetPacket packet)
    {
        stream.Write(packet.packetData, 0, packet.packetData.Length);
        stream.Flush();
    }

    private void SendData(int protocol)
    {
        if (!connected) return;

        NetPacket packet = new NetPacket(protocol);

        stream.Write(packet.packetData, 0, packet.packetData.Length);
        stream.Flush();
    }

    private void SendData(int protocol, string data)
    {
        if (!connected) return;

        NetPacket packet = new NetPacket(protocol, data);

        stream.Write(packet.packetData, 0, packet.packetData.Length);
        stream.Flush();
    }
    #endregion

    public void StartScene(Scene scene)
    {
        currentScene = scene;

        int num = (int)scene;
        string numStr = string.Format("{0:00}", num);

        Debug.Log($"{numStr}. {scene}");
        SceneManager.LoadScene($"{numStr}. {scene}");
    }


}
