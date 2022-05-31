using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    CardDragAndDrop cardDraAndDrop;
    DeckManager deckManager;

    public GameObject player1Panel;
    public GameObject player2Panel;
    public GameObject battlePanel;
    public GameObject touchBlock;
    public GameObject nextBtn;

    public List<RectTransform> player1BattleField = new List<RectTransform>();
    public List<RectTransform> player2BattleField = new List<RectTransform>();

    public List<CardHandler> player1CardList = new List<CardHandler>();
    public List<CardHandler> player2CardList = new List<CardHandler>();

    public int p1WinCount;
    public int p2WinCount;
    public int drawCount;

    private void Awake()
    {
        cardDraAndDrop = FindObjectOfType<CardDragAndDrop>();
        deckManager = FindObjectOfType<DeckManager>();
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

        nextBtn.SetActive(false);

        p1WinCount = 0;
        p2WinCount = 0;
        drawCount = 0;

        deckManager.player1CanDraw = true;
        deckManager.player2CanDraw = true;
    }

    public void Battle()
    {
        player1CardList = player1CardList.OrderBy(x => x.fieldNum).ToList();
        player2CardList = player2CardList.OrderBy(x => x.fieldNum).ToList();

        StartCoroutine(PlaceToBattleField());

        Fight();


        touchBlock.SetActive(false);
    }

    public void Fight()
    {
        for (int i = 0; i < 5; i++)
        {
            if((player1CardList[i].card.power.hp - player2CardList[i].card.power.attackPower <= 0) 
                && (player2CardList[i].card.power.hp - player1CardList[i].card.power.attackPower <= 0)) //p2->p1 Á×°í p1->p2Á×°í
            {
                drawCount++;
            }
            else if(player1CardList[i].card.power.hp - player2CardList[i].card.power.attackPower <= 0)  //p2->p1 Á×À½
            {
                p2WinCount++;
            }
            else if (player2CardList[i].card.power.hp - player1CardList[i].card.power.attackPower <= 0) //p1->p2 Á×À½
            {
                p1WinCount++;
            }
            else                                                                                        //µÑ´Ù¾ÈÁ×À½
            {
                drawCount++;
            }
        }

        if (p1WinCount == p2WinCount)        //¹«½ÂºÎ
        {
            Debug.LogError("draw");
        }
        else if (p1WinCount > p2WinCount)    //p1½Â
        {
            Debug.LogError("p1win");
        }
        else                                //p2½Â
        {
            Debug.LogError("p2win");
        }

        nextBtn.SetActive(true);
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
            touchBlock.SetActive(true);


            Battle();
        }
    }

    public void OnClickNext()
    {
        ResetData();

        nextBtn.SetActive(false);
        battlePanel.SetActive(false);

        player1Panel.SetActive(true);

        player1BattleField.Clear();
        player2BattleField.Clear();
    }

    IEnumerator PlaceToBattleField()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.5f);
            player1CardList[i].transform.SetParent(player1BattleField[i], true);
            player2CardList[i].transform.SetParent(player2BattleField[i], true);
        }
    }
}
