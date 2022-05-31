using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoOpZombieGenerator : MonoBehaviour
{
    public static CoOpZombieGenerator Instance;

    private UIManager uiManager;
    private NetClient netClient;

    public GameObject zombiePref;
    public GameObject strongZombiePref;
    public GameObject kingZombiePref;
    //public Vector3 spawnPos;
    public int basicMaxCount = 50;
    public int strongMaxCount = 20;
    public int kingMaxCount = 1;
    private float playTime = 0;


    public GameObject ground;

    private float minXpos;
    private float minZpos;
    private float maxXpos;
    private float maxZpos;

    private Queue<GameObject> basicQueue = new Queue<GameObject>();
    private Queue<GameObject> strongQueue = new Queue<GameObject>();

    private int currentZombieCount;
    private int currentStrongZombieCount;
    private int currentKingZombieCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        uiManager = UIManager.Instance;
        netClient = NetClient.Instance;

        Bounds bounds = ground.GetComponent<MeshRenderer>().bounds;

        minXpos = bounds.min.x + 5;
        minZpos = -40;
        maxXpos = bounds.max.x - 5;
        maxZpos = bounds.max.z - 5;
    }

    private void Update()
    {
        playTime += Time.deltaTime;
        uiManager.timeText.text = Mathf.Floor(playTime).ToString();
    }

    public void StartGenerate()
    {
        StartCoroutine(GenerateZombieCoroutine());
    }

    private IEnumerator GenerateZombieCoroutine()
    {
        Debug.Log("asf");
        float firstPhaseTime = 10f;
        float secondPhaseTime = 20f;
        float finalPhaseTime = 30f;

        float spawnDelay = 3f;


        while (playTime < firstPhaseTime)
        {
            
            for(int i = 0; i < 2; i++)
            {
                GenerateZombie(zombiePref, ZombieType.Basic);
            }
            yield return new WaitForSeconds(spawnDelay);
        }

        spawnDelay = 2f;

        while(playTime < secondPhaseTime)
        {
            GenerateZombie(zombiePref, ZombieType.Basic);
            GenerateZombie(strongZombiePref, ZombieType.Strong);

            yield return new WaitForSeconds(spawnDelay);
        }

        spawnDelay = 1f;

        while(playTime < finalPhaseTime)
        {
            GenerateZombie(strongZombiePref, ZombieType.Strong);
            GenerateZombie(kingZombiePref, ZombieType.King);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void GenerateZombie(GameObject pref, ZombieType type)
    {
        Queue<GameObject> queue;
        switch (type)
        {
            case ZombieType.Basic:
                if(currentZombieCount >= basicMaxCount)
                {
                    return;
                }
                currentZombieCount++;
                queue = basicQueue;
                break;
            case ZombieType.Strong:
                if (currentStrongZombieCount >= strongMaxCount)
                {
                    return;
                }
                currentStrongZombieCount++;
                queue = strongQueue;
                break;
            case ZombieType.King:
                if (currentKingZombieCount >= kingMaxCount)
                {
                    return;
                }
                currentKingZombieCount++;

                Instantiate(pref, SpawnPosition(), Quaternion.Euler(0, Random.Range(0, 360), 0));

                return;
            default:
                queue = new Queue<GameObject>();
                break;
        }

        //float xPos = Random.Range(minXpos + 1, maxXpos - 1);
        //float zPos = Random.Range(minZpos + 1, maxZpos - 1);

        //Vector3 spawnPos = new Vector3(xPos, 1, zPos);

        Vector3 spawnPos = SpawnPosition();
        float yRotation = Random.Range(0, 360);

        if (queue.Count == 0)
        {
            Instantiate(pref, spawnPos, Quaternion.Euler(0, yRotation, 0));
            // 1. pos, 2.zombieType(0, 1, 2)
            string pos = $"{spawnPos.x}:{spawnPos.y}:{spawnPos.z}:{type}";
            netClient.SendData(new NetPacket(NetProtocol.EVT_GENERATE_ZOMBIE));
        }
        else
        {
            GameObject zombie = queue.Dequeue();
            zombie.transform.position = spawnPos;
            zombie.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            zombie.SetActive(true);
        }
    }

    private Vector3 SpawnPosition()
    {
        float xPos = Random.Range(minXpos + 1, maxXpos - 1);
        float zPos = Random.Range(minZpos + 1, maxZpos - 1);

        return new Vector3(xPos, 1, zPos);
    }

    public static void EnqueueZombie(ZombieType type, GameObject zombie)
    {
        Queue<GameObject> queue;
        switch (type)
        {
            case ZombieType.Basic:
                Instance.currentZombieCount--;
                queue = Instance.basicQueue;
                break;
            case ZombieType.Strong:
                Instance.currentStrongZombieCount--;
                queue = Instance.strongQueue;
                break;
            default:
                queue = new Queue<GameObject>();
                break;
        }

        queue.Enqueue(zombie);

    }

    #region Net
    public void GenerateZombieAt(string zombieGen)
    {
        // x:y:z:type
        string[] info = zombieGen.Split(':');
        Vector3 spawnPos = new Vector3(CommanUtil.StrToFloat(info[0]), CommanUtil.StrToFloat(info[1]), CommanUtil.StrToFloat(info[2]));
        ZombieType type = (ZombieType)(int.Parse(info[3]));
        GameObject pref = null;
        switch(type)
        {
            case ZombieType.Basic:
                pref = zombiePref;
                break;
            case ZombieType.Strong:
                pref = strongZombiePref;
                break;
            case ZombieType.King:
                pref = kingZombiePref;
                break;
        }

        Instantiate(pref, spawnPos, Quaternion.Euler(0, Random.Range(0, 360), 0));
    }

    
    #endregion
}
