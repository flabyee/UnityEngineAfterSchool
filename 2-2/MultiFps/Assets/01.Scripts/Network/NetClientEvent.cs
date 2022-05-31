using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class NetClientEvent
{
    private NetClient netClient;

    public NetClientEvent()
    {
        netClient = NetClient.Instance;
    }
    public void OnEvent(NetPacket packet)
    {
        //Debug.Log("OnReadData : " + packet.protocol);


        switch (packet.protocol)
        {
            case NetProtocol.SYS_CLIENT_LIST:
                string[] clientNames = (string[])packet.PopObject();

                for (int i = 0; i < clientNames.Length; i++)
                {
                    if (i == clientNames.Length - 1) // Me
                    {
                        break;
                    }

                    ClientToken ct = netClient.AddClient(clientNames[i]);
                    ChatManager.Instance.OnUserJoin(ct.id, ct.clientName);
                }
                break;
            case NetProtocol.SYS_CLIENT_DISCONNECT:
                int disconnectedId = packet.PopInt();

                string dName = netClient.clients.Find(client => client.id == disconnectedId).clientName;

                //clients.Remove
                ChatManager.Instance.OnUserDisconnect(disconnectedId, dName);
                break;
            case NetProtocol.SYS_SET_HOST:
                netClient.isHost = true;
                ChatManager.Instance.InteractableStartButton();
                break;
            case NetProtocol.RES_NICKNAME:  // == UserJoined, first is Me
                ClientToken t = netClient.AddClient(packet.PopString());
                if (!netClient.idSet)
                {
                    this.netClient.listOrder = netClient.clients.Count - 1;

                    this.netClient.myId = t.id;
                    netClient.idSet = true;
                }

                //Debug.Log("my listorder : " + netClient.listOrder);

                ChatManager.Instance.OnUserJoin(t.id, t.clientName);
                break;

            case NetProtocol.RES_CHAT:
                // nickname&chat~
                string[] datas = packet.PopString().Split('&');

                StringBuilder chat = new StringBuilder();
                for (int i = 1; i < datas.Length; i++)
                {
                    chat.Append(datas[i]);
                    if (i > 1)
                    {
                        chat.Append("&");
                    }
                }

                ChatManager.Instance.OnReadChat(datas[0], chat.ToString());
                break;
            case NetProtocol.RES_GAME_START:
                int gameType = packet.PopInt();
                switch (gameType)
                {
                    case 0:

                        netClient.StartScene(NetClient.Scene.MultiplayScene);
                        break;
                    case 1:

                        netClient.StartScene(NetClient.Scene.MultiplayCoOpScene);
                        break;
                }
                break;
            case NetProtocol.RES_PLAYER_TRANSFORM:
                List<string[]> transformsList = (List<string[]>)packet.PopObject();

                //Debug.Log("PT : " + transformsList.Count + " : " + transformsList[0][0] + " : " + transformsList[1][0]);

                InGameManager.Instance.NetPlayersMove(transformsList);
                break;
            case NetProtocol.EVT_SHOOT_BULLET:
                string[] shootInfo = (string[])packet.PopObject();
                InGameManager.Instance.NetPlayerShoot(shootInfo);
                break;
            case NetProtocol.EVT_PLAYER_HIT:
                // [0] : hp, [1] = listOrder
                string[] hps = (string[]) packet.PopObject();
                InGameManager.Instance.NetPlayerHit(hps);
                break;
            case NetProtocol.EVT_PLAYER_JUMP:
                int listOrder = packet.PopInt();
                InGameManager.Instance.NetPlayerJump(listOrder);
                break;
            case NetProtocol.EVT_PLAYER_DEAD:
                InGameManager.Instance.NetPlayerDead(packet.PopInt());
                break;
            case NetProtocol.EVT_GENERATE_ZOMBIE:
                CoOpZombieGenerator.Instance.GenerateZombieAt(packet.PopString());
                break;
            default:
                break;
        }


    }
}
