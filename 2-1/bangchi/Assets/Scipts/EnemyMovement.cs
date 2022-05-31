using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    private GameManager gameManager = null;
    public float speed = 10f;

    public float maxHp = 0;
    public float hp = 0;

    public GameObject OnDamageEffect = null;

    public Canvas canvas = null;
    public Slider hpBar = null;

    private Slider bar = null;

    public bool isBoss = false;

    private void Awake()
    {
        isBoss = false;
        gameManager = FindObjectOfType<GameManager>();
        maxHp = ((int)Mathf.Round(10f * (Mathf.Pow(1.06f, 10f) - Mathf.Pow(1.06f, 10f + DataManager.Instance.stageLevel) / (1f - 1.06f)))) * 2;
        hp = ((int)Mathf.Round(10f * (Mathf.Pow(1.06f, 10f) - Mathf.Pow(1.06f, 10f + DataManager.Instance.stageLevel) / (1f - 1.06f)))) * 2;
        canvas = FindObjectOfType<Canvas>();

        speed = Random.Range(3, 5);

        float randomHp = Random.Range(1f, 2f);
        hp *= randomHp;
        maxHp *= randomHp;
    }
    void Start()
    {
       
        
    }

    void Update()
    {
        transform.Translate(-speed * Time.deltaTime, 0, 0);
        //if(transform.position.x < -12f)
        //{
        //    Destroy(gameObject);
        //}
        if(bar != null)
        {
            bar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1f, 0));
        }

        if(transform.position.x < -20f)
        {
            DataManager.Instance.enemyCount--;
            Destroy(bar.gameObject);
            gameManager.enemys.Remove(this);
            Destroy(gameObject);
        }
    }

    public void OnDamage()
    {

        if (hp > DataManager.Instance.power || hp < 0)
        {
            hp -= DataManager.Instance.power;
            bar.value = 1f * ((float)hp / (float)maxHp);
        }
        else
        {
            if(isBoss == true)
            {
                DataManager.Instance.StageLevelUp();
                Camera.main.backgroundColor = Random.ColorHSV();
            }
            Destroy(bar.gameObject);
            DataManager.Instance.KillEnemy();
            gameManager.enemys.Remove(this);
            Destroy(gameObject);
        }
        GameObject effect = Instantiate(OnDamageEffect, transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        Destroy(effect, 0.2f);

    }

    public void OnSkillOne()
    {
        if (hp > DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.1f)) || hp < 0)
        {
            hp -= DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.1f));
            bar.value = 1f * ((float)hp / (float)maxHp);
        }
        else
        {
            if (isBoss == true)
            {
                DataManager.Instance.StageLevelUp();
                Camera.main.backgroundColor = Random.ColorHSV();
            }
            Destroy(bar.gameObject);
            DataManager.Instance.KillEnemy();
            gameManager.enemys.Remove(this);
            Destroy(gameObject);
        }
        GameObject effect = Instantiate(OnDamageEffect, transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        Destroy(effect, 0.2f);
    }
    public void OnSkillTwo()
    {
        if (hp > DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.2f)) || hp < 0)
        {
            hp -= DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.2f));
            bar.value = 1f * ((float)hp / (float)maxHp);
        }
        else
        {
            if (isBoss == true)
            {
                DataManager.Instance.StageLevelUp();
                Camera.main.backgroundColor = Random.ColorHSV();
            }
            Destroy(bar.gameObject);
            DataManager.Instance.KillEnemy();
            gameManager.enemys.Remove(this);
            Destroy(gameObject);
        }
        GameObject effect = Instantiate(OnDamageEffect, transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        Destroy(effect, 0.2f);
    }
    public void OnSkillThree()
    {
        if (hp > DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.3f)) || hp < 0)
        {
            hp -= DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.3f));
            bar.value = 1f * ((float)hp / (float)maxHp);
        }
        else
        {
            if (isBoss == true)
            {
                DataManager.Instance.StageLevelUp();
                Camera.main.backgroundColor = Random.ColorHSV();
            }
            Destroy(bar.gameObject);
            DataManager.Instance.KillEnemy();
            gameManager.enemys.Remove(this);
            Destroy(gameObject);
        }
        GameObject effect = Instantiate(OnDamageEffect, transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        Destroy(effect, 0.2f);
    }
    public void OnSkillFore()
    {
        if (hp > DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.1f)) || hp < 0)
        {
            hp -= DataManager.Instance.power * (1 + (DataManager.Instance.skillLevel * 0.1f));
            bar.value = 1f * ((float)hp / (float)maxHp);
        }
        else
        {
            if (isBoss == true)
            {
                DataManager.Instance.StageLevelUp();
                Camera.main.backgroundColor = Random.ColorHSV();
            }
            Destroy(bar.gameObject);
            DataManager.Instance.KillEnemy();
            gameManager.enemys.Remove(this);
            Destroy(gameObject);
        }
        GameObject effect = Instantiate(OnDamageEffect, transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        Destroy(effect, 0.2f);
    }
    public void OnSkillFive()
    {

    }

    public void SpawnHpBar()
    {
        bar = Instantiate(hpBar, canvas.transform);

    }

}
