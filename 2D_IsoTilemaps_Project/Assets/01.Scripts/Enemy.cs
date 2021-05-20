using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    IsometricCharacterRenderer isoRenderer;
    Rigidbody2D rbody;
    SpriteRenderer renderer;



    public GameObject fireballPrefab;
    public GameObject player = null;
    private Transform searchCorTransform;

    private enum State
    {
       Idle,
       Chase,
       Die
    };
    State state = State.Idle;

    public float movementSpeed = 1f;

    public float attackCool = 2f;
    private float lastFireTime = 0f;

    private float angleRotateCool = 1f;
    private float lastRotateTime = 0;

    public int level = 1;
    public int hp;
    public int power;

    private static Vector3 N = Vector3.up;private static Vector3 NW = (Vector3.up + Vector3.left).normalized;private static Vector3 W = Vector3.left;private static Vector3 SW = (Vector3.down + Vector3.left).normalized;private static Vector3 S = Vector3.down;private static Vector3 SE = (Vector3.down + Vector3.right).normalized;private static Vector3 E = Vector3.right;private static Vector3 NE = (Vector3.up + Vector3.right).normalized;
    private Vector3[] dirs = { N, NW, W, SW, S, SE, E, NE };
    private float[] degrees = { 180, 225, 270, 315, 360, 0, 45, 90, 135 };

    float t;

    private void Awake()
    {
        player = GameObject.Find("Player_Isometric_Witch");
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        searchCorTransform = transform.GetChild(2);
    }

    private void Start()
    {
        hp = level * 5;
        power = level;

        t = 1f;
    }

    void FixedUpdate()
    {
        

        if(state == State.Idle)
        {
            if(Time.time > lastRotateTime)
            {
                lastRotateTime = Time.time + angleRotateCool;

                int index = Random.Range(0, 7);
                isoRenderer.lastDirection = IsometricCharacterRenderer.DirectionToIndex(dirs[index], 8);
                isoRenderer.SetDirection(dirs[index] * 0.001f);
                searchCorTransform.rotation = Quaternion.Euler(0, 0, degrees[index]);
            }
        }
        else if (state == State.Chase)
        {
            searchCorTransform.rotation = Quaternion.Euler(0, 0, degrees[isoRenderer.lastDirection]);

            Vector2 currentPos = rbody.position;
            Vector2 inputVector = new Vector2(player.transform.position.x - currentPos.x, player.transform.position.y - currentPos.y);
            inputVector = Vector2.ClampMagnitude(inputVector, 1);
            Vector2 movement = inputVector * movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;

            isoRenderer.SetDirection(movement);
            rbody.MovePosition(newPos);

            if (Time.time > lastFireTime)
            {
                Shoot();
                lastFireTime = Time.time + attackCool;
            }
        }
        else if(state == State.Die)
        {
            if(t > 0)
            {
                t -= Time.deltaTime;
                renderer.color = new Color(0, 0, 0, t);
            }
            else
            {
                DataManager.Instance.LevelCountUp();
                Destroy(gameObject);
            }
            
        }
        
    }

    private void Shoot()
    {
        for (int i = 0; i < 8; i++)
        {
            Instantiate(fireballPrefab, transform.position, Quaternion.identity).GetComponent<EnemyFireball>().SetMaster(gameObject);
        }
    }

    public void OnDamage()
    {
        if(hp > DataManager.Instance.fireballPower)
        {
            hp -= DataManager.Instance.fireballPower;
        }
        else
        {
            state = State.Die;
            isoRenderer.SetDirection(Vector2.zero);
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(state != State.Die && collision.gameObject.CompareTag("Player"))
        {
            state = State.Chase;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (state != State.Die && collision.gameObject.CompareTag("Player"))
        {
            state = State.Idle;
            isoRenderer.SetDirection(Vector2.zero);
            lastRotateTime = Time.time + angleRotateCool;
        }
    }
}
