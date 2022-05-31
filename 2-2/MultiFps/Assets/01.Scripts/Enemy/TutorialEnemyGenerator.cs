using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyGenerator : MonoBehaviour
{
    public static TutorialEnemyGenerator Instance;

    public GameObject zombiePref;
    public GameObject strongZombiePref;
    public Vector3 spawnPos;
    private float spawnDelay;
    public int basicMaxCount = 10;
    public int strongMaxCount = 2;

    public GameObject ground;

    private float minXpos;
    private float minZpos;
    private float maxXpos;
    private float maxZpos;

    private Queue<GameObject> basicQueue = new Queue<GameObject>();
    private Queue<GameObject> strongQueue= new Queue<GameObject>();

    private int currentZombieCount;
    private int currentStrongZombieCount;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Bounds bounds = ground.GetComponent<MeshRenderer>().bounds;
   
        minXpos = bounds.min.x;
        minZpos = bounds.min.z;
        maxXpos = bounds.max.x;
        maxZpos = bounds.max.z;

        spawnDelay = 3;
        StartCoroutine(GenerateZombieCoroutine());
    }

    private IEnumerator GenerateZombieCoroutine()
    {
        int count = 0;

        while (true)
        {
            count++;
            if (currentZombieCount < basicMaxCount)
            {
                GenerateZombie(zombiePref, ZombieType.Basic);
            }

            if (currentStrongZombieCount < strongMaxCount)
            {
                GenerateZombie(strongZombiePref, ZombieType.Strong);
            }

            yield return new WaitForSeconds(spawnDelay);
         }
    }

    private void GenerateZombie(GameObject pref,  ZombieType type)
    {
        Queue<GameObject> queue;
        switch (type)
        {
            case ZombieType.Basic:
                currentZombieCount++;
                queue = basicQueue;
                break;
            case ZombieType.Strong:
                currentStrongZombieCount++;
                queue = strongQueue;
                break;
            default:
                queue = new Queue<GameObject>();
                break;
        }

        float xPos = Random.Range(minXpos + 1, maxXpos -1);
        float zPos = Random.Range(minZpos + 1, maxZpos -1);

        Vector3 spawnPos = new Vector3(xPos, 1, zPos);
        float yRotation = Random.Range(0, 360);

        if (queue.Count == 0)
        {
            Instantiate(pref, spawnPos, Quaternion.Euler(0,yRotation,0));
        }
        else
        {
            GameObject zombie = queue.Dequeue();
            zombie.transform.position = spawnPos;
            zombie.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            zombie.SetActive(true);
        }
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

}
