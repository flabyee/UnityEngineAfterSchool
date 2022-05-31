using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class Clicker : MonoBehaviour
{
    GameManager gameManager = null;
  
    public float nextTimetoClick = 0f;

    public ShaderTest shader = null;

    public float skill1Cooltime = 4f;
    private float next1 = 0;
    public float skill2Cooltime = 5f;
    private float next2 = 0;
    public float skill3Cooltime = 6f;
    private float next3 = 0;
    public float skill4Cooltime = 7f;
    private float next4 = 0;
    public float skill5Cooltime = 10f;
    private float next5 = 0;

    [SerializeField]
    private Button oneB;
    [SerializeField]
    private Button twoB;
    [SerializeField]
    private Button threeB;
    [SerializeField]
    private Button foreB;
    [SerializeField]
    private Button fiveB;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        shader = GetComponent<ShaderTest>();
    }

    void Update()
    {
        UpdateUI();
    }

    private void OnClick()

    {
        if(Time.time >= nextTimetoClick)
        {
            if(gameManager.enemys.Count != 0)
            {
                StartCoroutine(ShaderOnOff());

                gameManager.enemys.OrderBy(x => x.transform.position.x).First().OnDamage();
                

                nextTimetoClick = Time.time + 0.1f;
            }
        }
    }

    void UpdateUI()
    {
        oneB.image.fillAmount = 1 - ((next1 - Time.time) / skill1Cooltime);
        twoB.image.fillAmount = 1 - ((next2 - Time.time) / skill2Cooltime);
        threeB.image.fillAmount = 1 - ((next3 - Time.time) / skill3Cooltime);
        foreB.image.fillAmount = 1 - ((next4 - Time.time) / skill4Cooltime);
        fiveB.image.fillAmount = 1 - ((next5 - Time.time) / skill5Cooltime);
    }
    IEnumerator InteractableOn(Button b, float t)
    {
        yield return new WaitForSeconds(t);
        b.interactable = true;
    }

    public void OnSkillOne()//낮은 체력
    {    
        if(Time.time >= next1)
        {
            next1 = Time.time + skill1Cooltime;

            gameManager.enemys.OrderBy(x => x.hp).First().OnSkillOne();

            oneB.interactable = false;
            StartCoroutine(InteractableOn(oneB, skill1Cooltime));
        }
    }

    public void OnSkillTwo()//가장 멀리
    {
        if (Time.time >= next2)
        {
            next2 = Time.time + skill2Cooltime;

            gameManager.enemys.OrderByDescending(x => x.transform.position.x).First().OnSkillTwo();

            twoB.interactable = false;
            StartCoroutine(InteractableOn(twoB, skill2Cooltime));
        }
    }
    public void OnSkillThree()//이속 빠른
    {
        
        if (Time.time >= next3)
        {
            next3 = Time.time + skill3Cooltime;

            gameManager.enemys.OrderBy(x => x.speed).First().OnSkillThree();

            threeB.interactable = false;
            StartCoroutine(InteractableOn(threeB, skill3Cooltime));
        }

    }
    public void OnSkillFore()//전체공격
    {
        if (Time.time >= next4)
        {
            next4 = Time.time + skill4Cooltime;

            //StartCoroutine(SkillForeCourtine((List<EnemyMovement>)gameManager.enemys.OrderBy(x => x.transform.position.x)));
            List<EnemyMovement> temp = new List<EnemyMovement>();
            foreach (var i in gameManager.enemys.OrderBy(x => x.transform.position.x))
            {
                temp.Add(i);
            }
            //StartCoroutine(SkillForeCourtine(temp));

            
            Debug.Log(temp.Count);
            foreB.interactable = false;
            StartCoroutine(InteractableOn(foreB, skill4Cooltime));
        }
        
    }

    //private IEnumerator SkillForeCourtine(List<EnemyMovement> temp)
    //{
        
    //}
    public void OnSkillFive()//쿨감
    {
        if (Time.time >= next5)
        {
            next5 = Time.time + skill5Cooltime;
            if (next1 - Time.time - 3f < 0.1) next1 = 0.1f + Time.time;
            if (next2 - Time.time - 3f < 0.1) next2 = 0.1f + Time.time;
            if (next3 - Time.time - 3f < 0.1) next3 = 0.1f + Time.time;
            if (next4 - Time.time - 3f < 0.1) next4 = 0.1f + Time.time;
            fiveB.interactable = false;
            StartCoroutine(InteractableOn(fiveB, skill5Cooltime));
        }
    }

    IEnumerator ShaderOnOff()
    {
        shader.UpdateOutline(true, Random.ColorHSV());
        yield return new WaitForSeconds(0.1f);
        shader.UpdateOutline(false, Random.ColorHSV());
    }
}
