using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum ZombieType
{
    Basic,
    Strong,
    King
}

public class EnemyController : MonoBehaviour
{

    public Animator animator;
    private Slider hpHUD;
    public NavMeshAgent navAgent;
    public Transform destinationTest;

    private CapsuleCollider hitAreaCollider;
    private EnemyAttackArea attackArea;

    public ZombieStat stat;

    private bool moving = false;
    private bool dead = false;
    private bool attacking = false;
    private bool playerInAttackArea = false;
    private PlayerController targetPlayerController;

    protected int hp;

    private void Awake()
    {
        if (hitAreaCollider == null)
        {
            hitAreaCollider = GetComponent<CapsuleCollider>();
            attackArea = GetComponent<EnemyAttackArea>();
        }
    }

    private void OnEnable()
    {
        
        SetHP();
        AddHUD();

        Invoke("MoveToRandomPosition", 2f);

        hitAreaCollider.enabled = true;
    }

    private void OnDisable()
    {

    }

    private void Update()
    {
        if (dead)
        {
            return;
        }

        if (navAgent.remainingDistance <= 0)
        {
            if (moving)
            {
                moving = false;
                float delay = Random.Range(3, 4);
                Invoke("MoveToRandomPosition", delay);
            }
        }

        animator.SetFloat("Speed", navAgent.velocity.magnitude);

    }

    private void MoveToRandomPosition()
    {
        moving = true;
        float radius = 100;
        Vector3 randomPos = Random.insideUnitSphere * radius;
        NavMeshHit hit;

        Vector3 destination;

        if (NavMesh.SamplePosition(randomPos, out hit, radius, 1))
        {
            destination = hit.position;
        }
        else
        {
            destination = transform.position - randomPos;
        }

        navAgent.SetDestination(destination);
    }

    private void LateUpdate()
    {
        UpdateHUDPosition();
    }

    private void UpdateHUDPosition()
    {
        if(!dead)
        {
            UIManager.Instance.UpdateHUDPosition(hpHUD.gameObject, transform.position + transform.up * 2);

        }
    }

    private void AddHUD()
    {
        hpHUD = UIManager.Instance.AddEnemyHUD().GetComponent<Slider>();
        hpHUD.gameObject.SetActive(false);
    }

    private void SetHP()
    {
        hp = stat.maxHP;
    }


    internal int OnHit(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            hp = 0; 
        }

        hpHUD.value = (float) hp / stat.maxHP;

        if (!hpHUD.gameObject.activeSelf)
        {
            hpHUD.gameObject.SetActive(true);
        }

        if (hp > 0)
        {

        }
        else if(hp == 0)
        {
            dead = true;
            
            Destroy(hpHUD.gameObject);

            hitAreaCollider.enabled = false;


            navAgent.isStopped = true;

            animator.SetTrigger("Dead");

            //StartCoroutine(DeadZombie(hp));
            Invoke("DestroyZombie", 3);

            return stat.score;
        }

        return -1;

    }


    private void DestroyZombie()
    {
        TutorialEnemyGenerator.EnqueueZombie(stat.zombieType, gameObject);

        gameObject.SetActive(false);
    }

    //private IEnumerator DeadZombie(int time)
    //{
    //    yield return new WaitForSeconds(time);
    //    Destroy(gameObject);
    //}
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerController>().isDead())
            {
                return;
            }

            if (attacking)
            {

            }
            else
            {
                if (playerInAttackArea)
                {
                    Attack(targetPlayerController);
                }
                else
                {
                    navAgent.SetDestination(other.transform.position);
                }
            }
        }
    }

    internal void Attack(PlayerController playerController)
    {
        if(attacking || playerController == null)
        {
            return;
        }

        if (playerController.isDead())
        {
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerController.transform.position - transform.position), 1f);

        targetPlayerController = playerController;
        //playerController.OnHit(zombieData.attackPower, transform.position);

        attacking = true;
        
        animator.SetTrigger("Attack");
        navAgent.isStopped = true;

    }

    internal void SetTarget()
    {
        playerInAttackArea = true;
    }

    internal void ResetTarget()
    {
        targetPlayerController = null;
        playerInAttackArea = false;
    }

    private void OnAttackEnd()
    {
        Invoke("AttackEndAction", stat.attackDelay);

    }

    private void AttackEndAction()
    {
        attacking = false;

        navAgent.isStopped = false;

        if (playerInAttackArea)
        {
            Attack(targetPlayerController);
        }
    }
}
