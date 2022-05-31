using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private const string RECORD_KEY = "RECORD_SCORE";

    public int currentScore = 0;
    public Camera uiCamera;
    public RectTransform canvas;
    public GameObject canvasUI;
    public GameObject player;
    public Image playerHPGauge;
    public Text playerHPText;
    public GameObject enemyHUDPref;
    private int playerMaxHP;

    public Text scoreText; 
    public Text recordScoreText; 
    public Text maxAmmoText;
    public Text currentAmmoText;

    public Text timeText;

    public GameObject hudRoot;

    public TransitionAnimationManager transitionManager;

    public GameObject optionUI;

    public Image endUI;
    public Text winnerText;
    public Text loserText;


    private int recordScore;
    private bool paused;

    private void Awake()
    {
        Instance = this;

        recordScore = PlayerPrefs.GetInt(RECORD_KEY);
        recordScoreText.text = "RecordScore : " + recordScore;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            optionUI.SetActive(!optionUI.activeSelf);

            if (optionUI.activeSelf)
            {
                paused = true;
                Cursor.lockState = CursorLockMode.None;
                //Time.timeScale = 0;
            }
            else
            {
                paused = false;
                Cursor.lockState = CursorLockMode.Locked;
                //Time.timeScale = 1;
            }
        }
    }

    public void OnOffCanvasUI(bool on)
    {
        canvasUI.SetActive(on);
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void InitPlayerHP(int maxHP)
    {
        playerMaxHP = maxHP;

        UpdatePlayerHP(maxHP);
    }

    public void UpdatePlayerHP(int currentHP)
    {
        playerHPGauge.fillAmount = (float) currentHP / playerMaxHP;
        playerHPText.text = "현재 체력 : " + currentHP;

    }

    public void SetMaxAmmo(int count)
    {
        maxAmmoText.text = "/ "+ count.ToString();
        currentAmmoText.text = count.ToString();
    }

    public void ChangeCurrentAmmo(int count)
    {
        currentAmmoText.text = count.ToString();
    }

    public void ChangeScore(int score)
    {
        currentScore += score;
        scoreText.text = "Score : " + currentScore.ToString();

        if (recordScore < currentScore)
        {
            PlayerPrefs.SetInt(RECORD_KEY, currentScore);
        }
    }

    public GameObject AddEnemyHUD()
    {
        GameObject hud = Instantiate(enemyHUDPref, hudRoot.transform);
        return hud;
    }

    public void UpdateHUDPosition(GameObject hud, Vector3 target)
    {
        Vector3 targetPos = target;

        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(targetPos);

        if (viewportPoint.z < 0)
        {
            return;
        }

        Vector2 position = new Vector2(viewportPoint.x * canvas.rect.width, viewportPoint.y * canvas.rect.height);

        hud.transform.localPosition = position;
        
    }

    public void RoundEnd(int winner, bool isMe)
    {
        endUI.gameObject.SetActive(true);
        StartCoroutine(ChangeColorCor(1.5f, endUI, Color.clear, new Color(0, 0, 0, 160f / 255f)));

        Text targetText;
        if(isMe)
        {
            targetText = winnerText;

            winnerText.gameObject.SetActive(true);
        }
        else
        {
            targetText = loserText;

            loserText.gameObject.SetActive(true);

            loserText.text = $"Loser\nWinner is {NetClient.Instance.clients[winner]}";
        }
        Color origin = targetText.color;
        origin.a = 0;

        StartCoroutine(ChangeColorCor(1.5f, targetText, origin, targetText.color));
    }

    private IEnumerator ChangeColorCor(float duration, MaskableGraphic target, Color from, Color to)
    {
        float elapsedTime = 0;
        target.color = from;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            target.color = Color.Lerp(from, to, elapsedTime / duration);

            yield return null;
        }
    }

    //private IEnumerator ChangeTextCor(float duration, Text image, float from, float to)
    //{
    //    float elapsedTime = 0;
    //    image.color = new Color(image.color.r, image.color.g,

    //    while (elapsedTime < duration)
    //    {
    //        elapsedTime += Time.deltaTime;

    //        image.color = Color.Lerp(from, to, elapsedTime / duration);

    //        yield return null;
    //    }
    //}
}
