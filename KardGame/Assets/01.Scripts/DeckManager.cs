using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;

    public Deck initialDeck;

    private Deck globalDeck;
    

    public List<CardHandler> cardsInHand;

    public GameObject player1Panel_handle;
    public GameObject player2Panel_handle;
    public DropArea player1HandleArea;
    public DropArea player2HandleArea;

    public void Start()
    {
        // Initial Deck에서 player Deck으로 Clone
        globalDeck = initialDeck.Clone();
        globalDeck.Shuffle();
    }

    public void Player1Draw()
    {
        // Draw 호출 되면 InstantiateCardObject 실행
        StartCoroutine(Draw1(globalDeck));
    }
    public void Player2Draw()
    {
        StartCoroutine(Draw2(globalDeck));
    }

    IEnumerator Draw1(Deck deck)
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.5f);
            InstantiateCardObject1(deck.Draw());
        }
    }
    IEnumerator Draw2(Deck deck)
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.5f);
            InstantiateCardObject2(deck.Draw());
        }
    }
    public void InstantiateCardObject1(Card cardData)
    {
        if (cardData == null) return;
        var cardObject = Instantiate(cardPrefab, player1Panel_handle.transform);
        cardObject.GetComponent<DropItem>().SetDroppedArea(player1HandleArea);
        // cardInHand에 넣고, CardHandler에서 initialize 진행
        CardHandler temp = cardObject.GetComponent<CardHandler>();
        cardsInHand.Add(temp);
        temp.Init(cardData);
    }
    public void InstantiateCardObject2(Card cardData)
    {
        if (cardData == null) return;
        var cardObject = Instantiate(cardPrefab, player2Panel_handle.transform);
        cardObject.GetComponent<DropItem>().SetDroppedArea(player2HandleArea);
        // cardInHand에 넣고, CardHandler에서 initialize 진행
        CardHandler temp = cardObject.GetComponent<CardHandler>();
        cardsInHand.Add(temp);
        temp.Init(cardData);
    }
}
