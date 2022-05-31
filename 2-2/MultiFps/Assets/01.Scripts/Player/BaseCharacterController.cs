using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float gravity;

    protected int hp;
    public int maxHP;

    protected float moveY;
    protected Vector3 moveDir;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {

    }

    public abstract void UpdateMoveDirection(Vector3 dir);
    public abstract void OnHit(int hp);
    public abstract void OnPlayerJump();
    public abstract void OnBleed(Vector3 hitPosition);


    void Update()
    {
        
    }

}
