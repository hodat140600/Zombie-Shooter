using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    Transform target;
    Animator aim;
    bool isDead = false;
    [SerializeField]
    public float distanceAtk = 2;
    [SerializeField]
    float turnSpeed = 5f;

    public float damageAmount = 35f;
    [SerializeField]
    float timeAttack = 2f;
    public bool canAttack = true;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        aim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (!isDead && !PlayerHealth.singleton.isDead)
        {
            if (distance < distanceAtk && canAttack)
            {
                AttackPlayer();
            }
            else if (distance > distanceAtk)
            {
                ChasePlayer();
            }
        }
        else 
        {
            DisableEnemy();
        }
    }

    public void DeathAnim()
    {
        isDead = true;
        aim.SetTrigger("isDead");
    }

    void ChasePlayer()
    {
        agent.updateRotation = true;
        agent.updatePosition = true;
        agent.SetDestination(target.position);
        aim.SetBool("isWalking", true);
        aim.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        agent.updateRotation = false;
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime) ;

        agent.updatePosition = false;
        aim.SetBool("isWalking", false);
        aim.SetBool("isAttacking", true);
        StartCoroutine(AttackTime());
    }

    void DisableEnemy()
    {
        canAttack = false;
        aim.SetBool("isWalking", false);
        aim.SetBool("isAttacking", false);
    }

    IEnumerator AttackTime()
    {
        canAttack = false;
        yield return new WaitForSeconds(1f);
        PlayerHealth.singleton.DamgePlayer(damageAmount);
        yield return new WaitForSeconds(timeAttack);
        canAttack = true;
    }
}
