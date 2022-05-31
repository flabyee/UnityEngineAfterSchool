using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public GameObject fireballPrefab;
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    public GameObject portalP;
    List<Transform> portals = new List<Transform>();

    public float movementSpeed = 1f;

    float portalCool = 0.5f;
    float nextTime = 0;
    public float fireCool = 10f;
    float lastFireTime = 0;
    float speedSkillCool = 30f;
    float lastSpeedTime = 0;

    GameObject temp;

    private Vector3 dir;

    private float horizontalInput;
    private float verticalInput;

    private static Vector3 N = Vector3.up;
    private static Vector3 NW = (Vector3.up + Vector3.left).normalized;
    private static Vector3 W = Vector3.left;
    private static Vector3 SW = (Vector3.down + Vector3.left).normalized;
    private static Vector3 S = Vector3.down;
    private static Vector3 SE = (Vector3.down + Vector3.right).normalized;
    private static Vector3 E = Vector3.right;
    private static Vector3 NE = (Vector3.up + Vector3.right).normalized;

    private Vector3[] dirs = {N, NW, W, SW, S, SE, E, NE };
    private void Awake()
    {
        portalP = GameObject.Find("Portals");

        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        
        portalP.GetComponentsInChildren<Transform>(portals);
        portals.RemoveAt(0);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 currentPos = rbody.position;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "HOS" && Time.time > nextTime)
        {
            portals.Remove(collision.transform);

            nextTime = Time.time + portalCool;
            Transform setTrn = portals[Random.Range(0, portals.Count)];
            transform.position = setTrn.position;

            portals.Add(collision.transform);
        }
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && Time.time > lastFireTime)
        {
            lastFireTime = Time.time + fireCool;
            int shootCount = 3 + DataManager.Instance.level;
            if(shootCount >= 15)
            {
                shootCount = 15;
            }
            for (int i = 0; i < shootCount; i++)
            {
                Instantiate(fireballPrefab, transform.position, Quaternion.identity).GetComponent<Fireball>().SetDir(dirs[isoRenderer.lastDirection]);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > lastSpeedTime)
        {
            lastSpeedTime = Time.time + speedSkillCool;
            movementSpeed *= 3f;
            Invoke("SpeedDown", 10f);
        }
    }

    void SpeedDown()
    {
        movementSpeed /= 3;
    }
}
