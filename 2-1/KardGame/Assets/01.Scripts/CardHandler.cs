using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    public Image cardIllust;
    public Text cardText;

    public Text atkText;
    public Text hpText;

    public Card card;

    public int fieldNum;

    public void Init(Card cardData)
    {
        card = cardData;

        if(card.power.illust != null)
        {
            cardIllust.sprite = card.power.illust;
        }

        cardText.text = card.power.cardName;

        atkText.text = card.power.attackPower.ToString();
        hpText.text = card.power.hp.ToString();
    }

    public void SetFieldNum(int num)
    {
        fieldNum = num;
    }

    //public void ClickCard()
    //{
    //    Debug.Log("click");
    //    Destroy(this.gameObject);
    //}
}
