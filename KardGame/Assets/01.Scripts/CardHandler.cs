using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    public Image cardIllust;
    public Text cardText;

    public Card card;

    public void Init()
    {
        cardIllust.sprite = card.power.illust;
        cardText.text = card.power.cardName;
    }

    public void ClickCard()
    {
        List<string> strList = new List<string>();
        string[] str = card.power.seqOnUse.Split('/');

        foreach(var item in str)
        {
            strList.Add(item);
        }

        switch(strList.Count)
        {
            case 1:
                break;
            case 2:
                if(strList[0] == "Attack")
                {
                    //adskdsfjnkfdsksadmfdslfmlsdfnaksdfkdladsnflsanflas
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                    //dsafkln;ksafsafdsakfaskjldflakfsdf
                }
                break;
            case 3:
                break;
        }

        Debug.Log("click");
        Destroy(this.gameObject);
    }
}
