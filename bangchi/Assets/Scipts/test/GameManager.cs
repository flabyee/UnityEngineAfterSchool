using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class GameManager : MonoBehaviour
{
    public BigInteger gold = 1;

    public Text goldText;

    public void OnClickButton()
    {
        gold *= 11;
        goldText.text = GetGoldText(gold);
        Debug.Log(gold);
    }

    void Start()
    {
        goldText.text = GetGoldText(gold);      
    }

    public string GetGoldText(BigInteger _gold)
    {
        string gText = string.Empty;

        gText = _gold.ToString();

        switch(gText.Length)
        {
            case 1: case 2: case 3:
                break;
            case 4: case 5: case 6:
                gText = gText.Remove(gText.Length - 3, 3);
                gText += 'K';
                break;
            case 7: case 8: case 9:
                gText = gText.Remove(gText.Length - 6, 6);
                gText += 'M';
                break;
            case 10: case 11: case 12:
                gText = gText.Remove(gText.Length - 9, 9);
                gText += 'B';
                break;
            case 13: case 14: case 15:
                gText = gText.Remove(gText.Length - 12, 12);
                gText += 'T';
                break;
            default:
                gText = string.Format("{0}.{1}{2}E + {3}", gText[0], gText[1], gText[2], gText.Length - 1);
                Debug.LogError(gText.Length);
                break;
        }

        return gText;
    }

    void Update()
    {


    }
}


































//msdn