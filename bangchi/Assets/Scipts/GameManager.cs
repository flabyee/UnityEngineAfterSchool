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

    void Start()
    {
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
            Instantiate(Enemy, new Vector2(10f, randomY), Quaternion.identity);
            yield return new WaitForSeconds(EnemySpawnTime);
        }
    }

    void textLoad()
    {
       
    }
}
