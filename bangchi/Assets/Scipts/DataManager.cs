using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics; //biginteger 사용하기 위해서

public class DataManager : MonoBehaviour
{
    //#region Singleton Patern

    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<DataManager>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newSingleton = new GameObject("singleton Class").AddComponent<DataManager>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<DataManager>();
        if(objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    public BigInteger gold = 0;
    public int stageLevel = 1;
    public int killCount = 0;
    public int attackSpeedLevel = 1;

    void Start()
    {
        
    }

    public string GetGoldText(BigInteger _gold)
    {
        string gText = string.Empty;

        gText = _gold.ToString();

        switch (gText.Length)
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

    public void KillEnemy()
    {
        killCount++;
        gold += stageLevel * stageLevel;

        if (killCount >= 10)
        {
            killCount = 0;
            stageLevel++;
        }
    }

    public void OnClickUpgradeButton()
    {
        if(gold > (100 * attackSpeedLevel * attackSpeedLevel) && attackSpeedLevel < 100)
        {
            attackSpeedLevel++;
            gold -= 100 * attackSpeedLevel * attackSpeedLevel;
        }
    }

    public void StageLevelReset()
    {
        stageLevel = 1;
    }
}
