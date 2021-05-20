using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics; //biginteger 사용하기 위해서
using System;
using System.IO;

[SerializeField]
public class PlayerData
{
    public int clickerLevel;
    public int stageLevel;
    public string gold;
    public int bossCount;
}

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
    public int enemyCount = 0;
    public int killCount = 0;
    public int power = 0;
    public int bossCount = 0;

    private LightTest light = null;

    public int clickPowerLevel = 1;

    public int skillLevel = 1;


    void Start()
    {
        LoadData();
        power = (int)((50 * Mathf.Pow(1.07f, clickPowerLevel - 1)) * 0.4f);
        light = FindObjectOfType<LightTest>();
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
        gold += (BigInteger)Mathf.Round(10f * (Mathf.Pow(1.06f, 10f) - Mathf.Pow(1.06f, 10f + stageLevel) / (1f - 1.06f)));

        if (killCount >= 10)
        {
            StageLevelUp();
            StartCoroutine(light.ChangeBackground());
        }
    }

    public void StageLevelUp()
    {
        killCount = 0;
        enemyCount = 0;
        stageLevel++;
        SaveData();
    }

    public void clickPowerLevelUp()
    {
        power = (int)((50 * Mathf.Pow(1.07f, clickPowerLevel - 1)) * 0.4f);
        if ((int)(50 * Mathf.Pow(1.07f, DataManager.Instance.clickPowerLevel - 1)) < gold)
        {
            gold -= (int)(50 * Mathf.Pow(1.07f, DataManager.Instance.clickPowerLevel - 1));
            clickPowerLevel++; 
        }
    }


    public void SaveData()
    {
        PlayerData playerData = new PlayerData();

        playerData.clickerLevel = clickPowerLevel;
        playerData.stageLevel = stageLevel;
        playerData.gold = gold.ToString();
        playerData.bossCount = bossCount;

        File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", JsonUtility.ToJson(playerData));
    }

    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(Application.persistentDataPath + "/PlayerData.json"));
            clickPowerLevel = playerData.clickerLevel;
            stageLevel = playerData.stageLevel;
            gold = BigInteger.Parse(playerData.gold);
            bossCount = playerData.bossCount;
        }
        else
        {
            SaveData();
        }
        
    }


}
