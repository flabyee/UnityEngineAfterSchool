using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetServerEvent
{
    private NetServer netServer;

    public NetServerEvent()
    {
        netServer = NetServer.Instance;
    }

    public void OnEvent(NetPacket packet, ClientToken token)
    {
        NetPacket broadcastPacket = new NetPacket();
        switch (packet.protocol)
        {
            case NetProtocol.REQ_NICKNAME: // Client 접속 시 첫 메시지
                string nickname = packet.PopString();
                token.clientName = nickname;

                broadcastPacket = new NetPacket(NetProtocol.RES_NICKNAME, nickname + "&" + token.id);
                //Debug.Log("Server C ID:" + token.id);
                netServer.SendClientList(token);

                if (netServer.hostId == -1)
                {
                    netServer.SetHost(token);
                }
                break;
            case NetProtocol.REQ_CHAT:
                string chat = packet.PopString();
                string newData = $"{token.clientName}&{chat}";
                broadcastPacket = new NetPacket(NetProtocol.RES_CHAT, newData);
                break;
            case NetProtocol.REQ_GAME_START:
                netServer.DontDestory();

                // gameType : 0 = MultiPlay, 1 = CoOpPlay
                int gameType = packet.PopInt();

                broadcastPacket = new NetPacket(NetProtocol.RES_GAME_START, gameType);

                for (int i = 0; i < netServer.clients.Count; i++)
                {
                    netServer.clients[i].listOrder = i;
                    netServer.transformList.Add(new string[2]);
                }

                switch(gameType)
                {
                    case 0:
                        netServer.currentScene = NetClient.Scene.MultiplayScene;
                        break;
                    case 1:
                        netServer.currentScene = NetClient.Scene.MultiplayCoOpScene;
                        break;
                }

                break;
            case NetProtocol.REQ_PLAYER_TRANSFORM:
                //Debug.Log("Struct Length" + packet.bodyLength);
                if (!netServer.transformSending)
                {
                    netServer.transformSending = true;
                    netServer.StartCoroutine(netServer.SendTransformCoroutine());
                }

                netServer.transformList[token.listOrder] = (string[])packet.PopObject();
                //string[] positions = str[0].Split(':');
                //string[] rotations = str[1].Split(':');

                //Debug.Log($"{positions[0]}:{positions[1]}:{positions[2]}");

                return;
            case NetProtocol.EVT_SHOOT_BULLET:
                netServer.BroadcastExcept(packet, token.listOrder);
                return;
            case NetProtocol.EVT_PLAYER_HIT:
                netServer.BroadcastExcept(packet, token.listOrder);
                return;
            case NetProtocol.EVT_PLAYER_JUMP:
                netServer.BroadcastExcept(packet, token.listOrder);
                return;
            case NetProtocol.EVT_PLAYER_DEAD:
                netServer.BroadcastExcept(packet, token.listOrder);
                return;
            case NetProtocol.EVT_GENERATE_ZOMBIE:
                netServer.BroadcastExcept(packet, token.listOrder);
                return;
            default:
                Debug.LogError("dd");
                break;
        }

        netServer.Broadcast(broadcastPacket);
    }
}
