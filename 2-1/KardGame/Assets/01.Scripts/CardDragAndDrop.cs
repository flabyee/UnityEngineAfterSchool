using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDragAndDrop : MonoBehaviour
{
    public DropArea player1FieldArea1;
    public DropArea player1FieldArea2;
    public DropArea player1FieldArea3;
    public DropArea player1FieldArea4;
    public DropArea player1FieldArea5;
    public DropArea player1HandleArea;
    public DropArea player1HoverArea;

    public RectTransform player1FieldRectParent1;
    public RectTransform player1FieldRectParent2;
    public RectTransform player1FieldRectParent3;
    public RectTransform player1FieldRectParent4;
    public RectTransform player1FieldRectParent5;
    public RectTransform player1HandleRectParent;
    public RectTransform player1HoverRectParent;



    public DropArea player2FieldArea1;
    public DropArea player2FieldArea2;
    public DropArea player2FieldArea3;
    public DropArea player2FieldArea4;
    public DropArea player2FieldArea5;
    public DropArea player2HandleArea;
    public DropArea player2HoverArea;

    public RectTransform player2FieldRectParent1;
    public RectTransform player2FieldRectParent2;
    public RectTransform player2FieldRectParent3;
    public RectTransform player2FieldRectParent4;
    public RectTransform player2FieldRectParent5;
    public RectTransform player2HandleRectParent;
    public RectTransform player2HoverRectParent;

    GameManager gameManager = null;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        player1FieldArea1.onLifted += ObjectLiftedFromPlayer1Field1;
        player1FieldArea1.onDropped += ObjectDroppedToPlayer1Field1;

        player1FieldArea2.onLifted += ObjectLiftedFromPlayer1Field2;
        player1FieldArea2.onDropped += ObjectDroppedToPlayer1Field2;

        player1FieldArea3.onLifted += ObjectLiftedFromPlayer1Field3;
        player1FieldArea3.onDropped += ObjectDroppedToPlayer1Field3;

        player1FieldArea4.onLifted += ObjectLiftedFromPlayer1Field4;
        player1FieldArea4.onDropped += ObjectDroppedToPlayer1Field4;

        player1FieldArea5.onLifted += ObjectLiftedFromPlayer1Field5;
        player1FieldArea5.onDropped += ObjectDroppedToPlayer1Field5;


        player1HandleArea.onLifted += ObjectLiftedFromPlayer1Handle;
        player1HandleArea.onDropped += ObjectDroppedToPlayer1Handle;

        player1HoverArea.onLifted += ObjectLiftedFromPlayer1Hover;
        player1HoverArea.onDropped += ObjectDroppedToPlayer1Hover;

        //-----------------------------------------------------------//

        player2FieldArea1.onLifted += ObjectLiftedFromPlayer2Field1;
        player2FieldArea1.onDropped += ObjectDroppedToPlayer2Field1;
        player2FieldArea2.onLifted += ObjectLiftedFromPlayer2Field2;
        player2FieldArea2.onDropped += ObjectDroppedToPlayer2Field2;
        player2FieldArea3.onLifted += ObjectLiftedFromPlayer2Field3;
        player2FieldArea3.onDropped += ObjectDroppedToPlayer2Field3;
        player2FieldArea4.onLifted += ObjectLiftedFromPlayer2Field4;
        player2FieldArea4.onDropped += ObjectDroppedToPlayer2Field4;
        player2FieldArea5.onLifted += ObjectLiftedFromPlayer2Field5;
        player2FieldArea5.onDropped += ObjectDroppedToPlayer2Field5;
        player2HandleArea.onLifted += ObjectLiftedFromPlayer2Handle;
        player2HandleArea.onDropped += ObjectDroppedToPlayer2Handle;
        player2HoverArea.onLifted += ObjectLiftedFromPlayer2Hover;
        player2HoverArea.onDropped += ObjectDroppedToPlayer2Hover;

    }

    private void ObjectLiftedFromPlayer1Field1(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HoverRectParent, true);
        gameManager.player1CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer1Field2(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HoverRectParent, true);
        gameManager.player1CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer1Field3(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HoverRectParent, true);
        gameManager.player1CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer1Field4(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HoverRectParent, true);
        gameManager.player1CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer1Field5(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HoverRectParent, true);
        gameManager.player1CardList.Remove(gameObject.GetComponent<CardHandler>());
    }

    private void ObjectDroppedToPlayer1Field1(DropArea area, GameObject gameObject)
    {
        if(player1FieldRectParent1.childCount == 0)
        {
            gameObject.transform.SetParent(player1FieldRectParent1, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 1;
            gameManager.player1CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player1HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }
    }
    private void ObjectDroppedToPlayer1Field2(DropArea area, GameObject gameObject)
    {
        if (player1FieldRectParent2.childCount == 0)
        {
            gameObject.transform.SetParent(player1FieldRectParent2, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 2;
            gameManager.player1CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player1HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }

    }
    private void ObjectDroppedToPlayer1Field3(DropArea area, GameObject gameObject)
    {
        if (player1FieldRectParent3.childCount == 0)
        {
            gameObject.transform.SetParent(player1FieldRectParent3, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 3;
            gameManager.player1CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player1HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }

    }
    private void ObjectDroppedToPlayer1Field4(DropArea area, GameObject gameObject)
    {
        if (player1FieldRectParent4.childCount == 0)
        {
            gameObject.transform.SetParent(player1FieldRectParent4, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 4;
            gameManager.player1CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player1HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }
    }
    private void ObjectDroppedToPlayer1Field5(DropArea area, GameObject gameObject)
    {

        if (player1FieldRectParent5.childCount == 0)
        {
            gameObject.transform.SetParent(player1FieldRectParent5, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 5;
            gameManager.player1CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player1HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }

    }
    private void ObjectLiftedFromPlayer1Handle(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HoverRectParent, true);
    }
    private void ObjectDroppedToPlayer1Handle(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HandleRectParent, true);

        gameObject.GetComponent<DropItem>().IsHandle();
    }
    private void ObjectLiftedFromPlayer1Hover(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HoverRectParent, true);
    }
    private void ObjectDroppedToPlayer1Hover(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player1HandleRectParent, true);

        gameObject.GetComponent<DropItem>().IsHandle();
    }

    //--------------------------------------------------------------//

    private void ObjectLiftedFromPlayer2Field1(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HoverRectParent, true);
        gameManager.player2CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer2Field2(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HoverRectParent, true);
        gameManager.player2CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer2Field3(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HoverRectParent, true);
        gameManager.player2CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer2Field4(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HoverRectParent, true);
        gameManager.player2CardList.Remove(gameObject.GetComponent<CardHandler>());
    }
    private void ObjectLiftedFromPlayer2Field5(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HoverRectParent, true);
        gameManager.player2CardList.Remove(gameObject.GetComponent<CardHandler>());
    }

    private void ObjectDroppedToPlayer2Field1(DropArea area, GameObject gameObject)
    {
        if(player2FieldRectParent1.childCount == 0)
        {
            gameObject.transform.SetParent(player2FieldRectParent1, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 1;
            gameManager.player2CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player2HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }
    }
    private void ObjectDroppedToPlayer2Field2(DropArea area, GameObject gameObject)
    {
        if (player2FieldRectParent2.childCount == 0)
        {
            gameObject.transform.SetParent(player2FieldRectParent2, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 2;
            gameManager.player2CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player2HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }

    }
    private void ObjectDroppedToPlayer2Field3(DropArea area, GameObject gameObject)
    {
        if (player2FieldRectParent3.childCount == 0)
        {
            gameObject.transform.SetParent(player2FieldRectParent3, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 3;
            gameManager.player2CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player2HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }

    }
    private void ObjectDroppedToPlayer2Field4(DropArea area, GameObject gameObject)
    {
        if (player2FieldRectParent4.childCount == 0)
        {
            gameObject.transform.SetParent(player2FieldRectParent4, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 4;
            gameManager.player2CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player2HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }
    }
    private void ObjectDroppedToPlayer2Field5(DropArea area, GameObject gameObject)
    {

        if (player2FieldRectParent5.childCount == 0)
        {
            gameObject.transform.SetParent(player2FieldRectParent5, true);

            CardHandler temp = gameObject.GetComponent<CardHandler>();
            temp.fieldNum = 5;
            gameManager.player2CardList.Add(temp);

            gameObject.GetComponent<DropItem>().IsField();
        }
        else
        {
            gameObject.transform.SetParent(player2HandleRectParent, true);

            gameObject.GetComponent<DropItem>().IsHandle();
        }

    }
    private void ObjectLiftedFromPlayer2Handle(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HoverRectParent, true);
    }
    private void ObjectDroppedToPlayer2Handle(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HandleRectParent, true);

        gameObject.GetComponent<DropItem>().IsHandle();
    }
    private void ObjectLiftedFromPlayer2Hover(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HoverRectParent, true);
    }
    private void ObjectDroppedToPlayer2Hover(DropArea area, GameObject gameObject)
    {
        gameObject.transform.SetParent(player2HandleRectParent, true);

        gameObject.GetComponent<DropItem>().IsHandle();
    }
}
