using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    protected NetClient netClient;

    public GameObject playerPref;
    protected int playerCount;
    protected List<PlayerController> playerList = new List<PlayerController>();

    public Transform[] spawnPositions;

    protected int listOrder = -1;

    public float interpolateConstant = 1f;

    public int deadPlayerCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        netClient = NetClient.Instance;

        StartCoroutine(StartRoundCor());
    }

    void Update()
    {
        
    }

    private IEnumerator StartRoundCor()
    {
        yield return new WaitForSeconds(1.5f);

        // Player 수, Player 생성 위치, 플레이어 중 누가 나인지
        playerCount = netClient.clients.Count;

        listOrder = netClient.listOrder;

        for (int i = 0; i < playerCount; i++)
        {
            GeneratePlayer(i, i == listOrder);

            if (i == listOrder)
            {
                playerList[i].StartCamera();
                playerList[i].transform.Find("PlayerHit").tag = "PlayerHit";
            }
            else
            {
                playerList[i].transform.Find("PlayerHit").tag = "NetPlayerHit";
            }

            playerList[i].StartRotate(i == listOrder, i);
        }

        UIManager.Instance.OnOffCanvasUI(true);

        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < playerCount; i++)
        {
            playerList[i].StartGame(i == listOrder);
        }

        StartCallback();
    }

    public virtual void StartCallback()
    {

    }

    private void GeneratePlayer(int index, bool isMe)
    {
        GameObject player = Instantiate(playerPref, spawnPositions[index].position, Quaternion.identity);

        PlayerController pc = (isMe ? player.GetComponent<PlayerController>() : player.GetComponent<NetPlayerController>());
        pc.enabled = true;

        playerList.Add(player.GetComponent<PlayerController>());
    }

    public void CheckRoundEnd()
    {
        if(deadPlayerCount == playerList.Count - 1)
        {
            RoundWinner();
        }
    }

    private void RoundWinner()
    {
        int roundWinner = -1;
        for(int i = 0; i < playerList.Count; i++)
        {
            if(!playerList[i].dead)
            {
                roundWinner = i;
                break;
            }
        }

        UIManager.Instance.RoundEnd(roundWinner, roundWinner == listOrder);
    }
    
    

    #region NetEvent

    public void NetPlayersMove(List<string[]> transformsList)
    {
        for(int i = 0; i < transformsList.Count; i++)
        {
            if (i == listOrder)
            {
                continue;
            }
            // trans[0] : pos String, trans[1] rot String
            string[] trans = transformsList[i];

            if(trans[0] == null)
            {
                continue;
            }

            string[] posStrs = trans[0].Split(':');
            string[] rotStrs = trans[1].Split(':');

            Vector3 pos = new Vector3(float.Parse(posStrs[0]), float.Parse(posStrs[1]), float.Parse(posStrs[2]));
            //Quaternion rot = Quaternion.Euler(float.Parse(rotStrs[0]), float.Parse(rotStrs[1]), float.Parse(rotStrs[2]));
            float xAxis = float.Parse(rotStrs[0]);
            float yAxis = float.Parse(rotStrs[1]);

            Vector3 offset = pos - playerList[i].transform.position;

            // 진동 방지(와리가리)
            if(offset.magnitude < interpolateConstant)
            {
                playerList[i].UpdateMoveDirection(Vector3.zero);
            }
            else
            {
                playerList[i].UpdateMoveDirection(offset);

            }

            playerList[i].playerAim.UpdateNetPlayerRotation(xAxis, yAxis);

        }
    }

    public void NetPlayerShoot(string[] shootInfo)
    {
        string[] posStrs = shootInfo[0].Split(':');
        Vector3 bulletSpawnPos = new Vector3(float.Parse(posStrs[0]), float.Parse(posStrs[1]), float.Parse(posStrs[2]));

        string[] dirStrs = shootInfo[1].Split(':');
        Vector3 shootDirection = new Vector3(float.Parse(dirStrs[0]), float.Parse(dirStrs[1]), float.Parse(dirStrs[2]));

        string[] ammoAndListOrderStrs = shootInfo[2].Split(':');
        int ammo = int.Parse(ammoAndListOrderStrs[0]);
        int listOrder = int.Parse(ammoAndListOrderStrs[1]);

        playerList[listOrder].raycastAim.weapon.ShootByOther(shootDirection, bulletSpawnPos, ammo);
    }

    public void NetPlayerHit(string[] hitInfo)
    {
        int hp = int.Parse(hitInfo[0]);
        int lo = int.Parse(hitInfo[1]);

        playerList[lo].OnHit(hp);
    }

    public void NetPlayerDead(int lo)
    {
        deadPlayerCount++;
        playerList[lo].OnDead();
    }

    public void NetPlayerJump(int lo)
    {
        playerList[lo].jumpTrigger = true;
    }


    #endregion
}
