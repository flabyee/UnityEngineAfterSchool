using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;

    public Deck initialDeck;
    private Deck playerDeck;

    public List<CardHandler> cardsInHand;

    public Canvas canvas;

    public void Start()
    {
        // Initial Deck에서 player Deck으로 Clone
        playerDeck = initialDeck.Clone();
    }

    public void Draw()
    {
        // Draw 호출 되면 InstantiateCardObject 실행
        InstantiateCardObject(playerDeck.Draw());
    }

    public void InstantiateCardObject(Card cardData)
    {
        var cardObject = Instantiate(cardPrefab, canvas.transform);
        // cardInHand에 넣고, CardHandler에서 initialize 진행
        CardHandler cardHandler = cardObject.GetComponent<CardHandler>();
        cardHandler.card = cardData;
        cardsInHand.Add(cardHandler);
        cardHandler.Init();
    }
}
