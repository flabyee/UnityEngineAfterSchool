using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetProtocol
{
    public const int SYS_CLIENT_LIST = -1;
    public const int SYS_CLIENT_DISCONNECT = -2;
    public const int SYS_SET_HOST = -3;
    public const int EVT_SHOOT_BULLET = -4; // req, res 공용으로 사용
    public const int EVT_PLAYER_HIT = -5;
    public const int EVT_PLAYER_JUMP = -6;
    public const int EVT_PLAYER_DEAD = -7;
    public const int EVT_GENERATE_ZOMBIE = -8;

    public const int REQ_NICKNAME = 1;
    public const int REQ_CHAT= 2;
    public const int REQ_GAME_START= 3;
    public const int REQ_PLAYER_TRANSFORM = 4;


    public const int RES_NICKNAME = 51;
    public const int RES_CHAT= 52;
    public const int RES_GAME_START= 53;
    public const int RES_PLAYER_TRANSFORM = 54;

}
