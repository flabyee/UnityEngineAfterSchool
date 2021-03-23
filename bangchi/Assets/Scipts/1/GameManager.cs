using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject EnemyPrefabs = null;
    public GameObject BulletPrefabs = null;
    public GameObject ShotPos = null;

    private float enemySpawnY = 0f;

    private float d = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        d += Time.deltaTime;
        if(d > 0.1f)
        {
            Instantiate(BulletPrefabs, new Vector2(-8f, ShotPos.transform.position.y), Quaternion.identity);
            d = 0;
        }
    }

    IEnumerator spawnEnemy()
    {
        while(true)
        {
            enemySpawnY = Random.Range(-4f, 4f);
            Instantiate(EnemyPrefabs, new Vector2(8, enemySpawnY), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }

}
