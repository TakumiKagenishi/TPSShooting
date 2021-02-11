using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    public bool moveEnabled = true;

    [SerializeField]
    int maxHp = 100;

    [SerializeField]
    int attackInterval = 2;

    [SerializeField]
    float deadTime = 3;

    GameObject target;

    bool attacking = false;
    int hp;
    float moveSpeed;
    Animator animator;
    BoxCollider boxCollider;
    Rigidbody rigidBody;
    NavMeshAgent agent;
    Shot shot;

    public int Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);

            if(hp <= 0)
            {
                StartCoroutine(Dead());
            }
        }

        get
        {
            return hp;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        rigidBody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        shot = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Shot>();
        InitCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveEnabled)
        {
            Move();
        }

        else
        {
            Stop();
        }
    }

    void InitCharacter()
    {
        Hp = maxHp;
        moveSpeed = agent.speed;
    }

    void Move()
    {
        agent.speed = moveSpeed;
        animator.SetFloat("Speed", agent.speed, 0.1f, Time.deltaTime);
        agent.destination = target.transform.position;
        rigidBody.velocity = agent.desiredVelocity;
    }

     void Stop()
    {
        agent.speed = 0;
        animator.SetFloat("Speed", agent.speed, 0.1f, Time.deltaTime);
    }

    IEnumerator Dead()
    {
        moveEnabled = false;
        Stop();
        animator.SetTrigger("Dead");
        boxCollider.enabled = false;
        rigidBody.isKinematic = true;
        yield return new WaitForSeconds(deadTime);
        Destroy(gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            StartCoroutine(AttackTimer());
        }
    }

    IEnumerator AttackTimer()
    {
        if(!attacking)
        {
            attacking = true;
            moveEnabled = false;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackInterval);
            attacking = false;
            moveEnabled = true;
        }

        yield return null;
    }
}
