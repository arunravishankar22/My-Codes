using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    public float chaseRange;
    private float distance;
    public float turnSpeed;
    private bool attack;
    private bool move;
    private bool Bite;
    public static bool dead;
    private Animator anime;
    public float damage;


    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        anime = this.GetComponent<Animator>();
    }
    void Update()
    {
        if (target != null)
        {
            distance = Vector3.Distance(this.transform.position, target.position);

            EnemyState(distance);
            updateAnime();
        }
    }

    void updateAnime()
    {
        anime.SetBool("Idel", move);
        anime.SetBool("Attack",attack);
        anime.SetBool("bite", Bite);
        //anime.SetBool("dead", dead);
        if(move)
        {
            anime.SetFloat("Distance", Mathf.Clamp(distance,0,10));
        }
    }

    private void EnemyState(float distance)
    {
        if (distance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            attack = true;
            target.GetComponent<player>().canHeal = false;
            lookAt();
        }
        else if (distance <= chaseRange)
        {
            agent.SetDestination(target.position);
            target.GetComponent<player>().canHeal = true;
            attack = false;
            move = false;
            Bite = true;
            agent.isStopped = false;
        }
        else if(distance>=chaseRange)
        {
            move = true;
            target.GetComponent<player>().canHeal = true;
            attack = false;
            
        }
    }

    private void lookAt()
    {
        Vector3 dir = (target.transform.position - this.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, chaseRange);
    }

    public void RedusePlayerHealth()
    {
        if(target.GetComponent<player>().TakeDamage(damage))
        {
            Time.timeScale = 0;
        }

    }

}
