using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject Enemy = null;
    public float EnemySpawnTime = 1f;

    public Text goldText;
    public Text stageText;
    public Text attackSpeedLevelText;

    public GameObject Player = null;

    private void Awake()
    {
        spawnPlayer();
    }
    void Start()
    {
        textLoad();
        StartCoroutine(SpawnEnemy());
        
    }

    void Update()
    {

    }

    IEnumerator SpawnEnemy()
    {
        float randomY = 0f;
        while(true)
        {
            randomY = Random.Range(-4f, 4f);
            Instantiate(Enemy, new Vector3(10f, randomY, 0), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnTime);
        }
    }


    public void textLoad()
    {
        goldText.text = DataManager.Instance.GetGoldText(DataManager.Instance.gold);
        stageText.text = string.Format("{0}스테이지", DataManager.Instance.stageLevel);
        attackSpeedLevelText.text = (DataManager.Instance.attackSpeedLevel).ToString();
    }

    public void spawnPlayer()
    {
        Instantiate(Player, new Vector3(-10, 0, 0), Quaternion.identity);
    }

    
}
