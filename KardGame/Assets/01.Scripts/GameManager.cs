using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    CardDragAndDrop cardDraAndDrop;

    public GameObject player1Panel;
    public GameObject player2Panel;
    public GameObject battlePanel;

    public List<CardHandler> player1CardList = new List<CardHandler>();
    public List<CardHandler> player2CardList = new List<CardHandler>();

    public int p1WinCount;
    public int p2WinCount;
    public int drawCount;

    private void Awake()
    {
        cardDraAndDrop = FindObjectOfType<CardDragAndDrop>();
    }

    void Start()
    {
        player1Panel.SetActive(true);

        ResetData();
    }

    void ResetData()
    {
        player1CardList.Clear();
        player2CardList.Clear();

        

        p1WinCount = 0;
        p2WinCount = 0;
        drawCount = 0;
    }

    public void Battle()
    {
        player1CardList = (List<CardHandler>)player1CardList.OrderBy(x => x.fieldNum);

        for (int i = 0; i < 5; i++)
        {
            Fight(player1CardList[i], player2CardList[i]);
        }


    }

    public void Fight(CardHandler p1, CardHandler p2)
    {
        //bool p1IsDie;
        //bool p2IsDie;

        //if(p2.power.hp - p1.power.attackPower > 0)
        //{
        //    p2IsDie = false;
        //}
        //else
        //{
        //    p2IsDie = true;
        //}

        //if (p1.power.hp - p2.power.attackPower > 0)
        //{
        //    p1IsDie = false;
        //}
        //else
        //{
        //    p1IsDie = true;
        //}

        //if((p1IsDie && p2IsDie) || (!p1IsDie && !p2IsDie))
        //{
        //    drawCount++;
        //}
        //else if(p1IsDie)
        //{
        //    p2WinCount++;
        //}
        //else if(p2IsDie)
        //{
        //    p1WinCount++;
        //}
    }




    public void OnClickPlayer1TurnEnd()
    {
        if (cardDraAndDrop.player1HandleRectParent.childCount == 0)
        {
            player1Panel.SetActive(false);

            player2Panel.SetActive(true);
        }
    }
    public void OnClickPlayer2TurnEnd()
    {
        if (cardDraAndDrop.player2HandleRectParent.childCount == 0)
        {
            player2Panel.SetActive(false);

            battlePanel.SetActive(true);

            Battle();
        }
    }

    public void OnClickBattleEnd()
    {

    }
}
