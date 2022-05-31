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
    public Text clickPowerLevelText;

    public GameObject playerPrefab = null;

    private GameObject player = null;

    public GameObject selectedEnemy = null;

    public List<EnemyMovement> enemys = new List<EnemyMovement>();

    private void Awake()
    {
       // SpawnPlayer();
    }
    void Start()
    {
        textLoad();
        StartCoroutine(SpawnEnemy());
        Camera.main.backgroundColor = Random.ColorHSV();
    }

    void Update()
    {
        textLoad();
    }

    IEnumerator SpawnEnemy()
    {
        float randomY = 0f;
        while (true)
        {
            if (enemys.Count < 8 && DataManager.Instance.enemyCount < 10 && DataManager.Instance.stageLevel % 10 != 0)
            {
                randomY = Random.Range(-4f, 4f);
                EnemyMovement e = (Instantiate(Enemy, new Vector3(8f, randomY, 0), Quaternion.identity)).GetComponent<EnemyMovement>();
                enemys.Add(e);
                e.SpawnHpBar();
                DataManager.Instance.enemyCount++;
            }
            else if(DataManager.Instance.stageLevel % 10 == 0 && DataManager.Instance.bossCount == DataManager.Instance.stageLevel / 10 - 1)
            {
                EnemyMovement e = (Instantiate(Enemy, new Vector3(8f, 0, 0), Quaternion.identity)).GetComponent<EnemyMovement>();
                enemys.Add(e);
                e.SpawnHpBar();
                e.hp *= 10;
                e.maxHp *= 10;
                e.gameObject.transform.localScale = new Vector3(2, 2, 1);
                e.isBoss = true;
                DataManager.Instance.bossCount++;
            }
            yield return new WaitForSeconds(EnemySpawnTime);


        }
    }

    public void textLoad()
    {
        goldText.text = DataManager.Instance.GetGoldText(DataManager.Instance.gold);
        
        clickPowerLevelText.text = string.Format("레벨 : {0}\n파워 : {1}", DataManager.Instance.clickPowerLevel, DataManager.Instance.power);
        if(DataManager.Instance.stageLevel % 10 == 0)
        {
            stageText.text = string.Format("{0}-{1}스테이지", (DataManager.Instance.stageLevel / 10), 10);
        }
        else
        {
            stageText.text = string.Format("{0}-{1}스테이지", (DataManager.Instance.stageLevel - DataManager.Instance.stageLevel % 10) / 10 + 1, DataManager.Instance.stageLevel % 10);
        }
    }

    //public void SpawnPlayer()
    //{
    //    player = Instantiate(playerPrefab, new Vector3(-10, 0, 0), Quaternion.identity);
    //}

    //public void GameReset()
    //{
    //    StartCoroutine(GameResetC());
    //    textLoad();
    //}

    //public IEnumerator GameResetC()
    //{
    //    Destroy(player);
    //    foreach(EnemyMovement enemyMovement in FindObjectsOfType<EnemyMovement>())
    //    {
    //        Destroy(enemyMovement.gameObject);
    //    }
    //    foreach(BulletMovement bulletMovement in FindObjectsOfType<BulletMovement>())
    //    {
    //        Destroy(bulletMovement.gameObject);
    //    }
    //    Time.timeScale = 0;
    //    yield return new WaitForSecondsRealtime(3f);
        
    //    SpawnPlayer();
    //    Time.timeScale = 1;
    //}

    public void OnClickClickPowerLevelUp()
    {
        DataManager.Instance.clickPowerLevelUp();
    }
}
