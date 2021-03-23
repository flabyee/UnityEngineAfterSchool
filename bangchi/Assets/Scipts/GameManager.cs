using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class GameManager : MonoBehaviour
{
    public List<int> goldList = new List<int>();
    public List<int> goldList100 = new List<int>();
    public Text haveMoney;

    public string empty = string.Empty;
    public string three = string.Empty;
    public string maxMoney = string.Empty;

    public BigInteger money = 1;


    public void OnClickButton()
    {
        money *= 123;

        GetGoldText();
        for (int i = 0; i < goldList.Count; i++)
        {
            if (i == 0)
                empty = goldList[i].ToString() + "원";
            else if (i == 1)
                empty = goldList[i].ToString() + "만" + empty;
            else if (i == 2)
                empty = goldList[i].ToString() + "억" + empty;
            else
                empty = goldList[i].ToString() + empty;

        }

        GetGoldText100();

        for (int i = 0; i < goldList100.Count; i++)
        {
            if (goldList100.Count < 2) { }
            if (i == 0)
                three += "," + goldList100[i].ToString();
            else if (i == 1)
                three += "," + goldList100[i].ToString();
            else if (i == 2)
                three += "," + goldList100[i].ToString();
            else
                three += "," + goldList100[i].ToString();
        }

        Debug.Log(three);
        Debug.Log(empty);
        //Debug.Log(maxMoney);
        empty = string.Empty;
        goldList.Clear();
        goldList100.Clear();
    }

    public void GetGoldText()
    {
        BigInteger moneyCopy = money;

        while (moneyCopy > 0)
        {
            goldList.Add((int)(moneyCopy % 10000));
            moneyCopy /= 10000;
        }
    }

    public void GetGoldText100()
    {
        BigInteger moneyCopy = money;
        goldList100.Add((int)(moneyCopy % 1000));
        moneyCopy /= 1000;
    }
    // Start is called before the first frame update
    void Start()
    {
        

       
        
        
    }

    void Update()
    {
        
        //if (goldList.Count == 1)
        //{
        //    haveMoney.text = string.Format("{0}원", gold);
        //}
        //else if (goldList.Count == 2)
        //{
        //    haveMoney.text = string.Format("{0}만{1}원", two, one);
        //}
        //else if(goldList.Count == 3)
        //{
        //    haveMoney.text = string.Format("{0}만{1}원", two, one);
        //}




    }

    
}


































//msdn