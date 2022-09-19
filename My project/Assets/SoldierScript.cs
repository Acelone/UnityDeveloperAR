
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool Active, HoldBall, Move, Caught, attacker,player;
    GameObject[] Ball,playerGoal,enemyGoal;
    private Animator anim;
    float NTime;
    AnimatorStateInfo animStateInfo;

    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration =100f;
    }
    void Awake()
    {
        Active=true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   anim.SetBool("Active", Active);
        anim.SetBool("Attacker", attacker);
        if(attacker)
        {   Move=true;
            anim.SetBool("Move", Move);
            animStateInfo = anim.GetCurrentAnimatorStateInfo (0);
            NTime = animStateInfo.normalizedTime;
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jog")&&NTime > 0.35f)
            {
                agent.speed = 20f*1.5f*Time.deltaTime;
                Ball = GameObject.FindGameObjectsWithTag("Ball");
                agent.SetDestination(Ball[0].transform.position);
                
            }
            if(anim.GetCurrentAnimatorStateInfo(0).IsName("dribble")&&NTime > 0.1f)
            {
                agent.speed = 20f*0.75f*Time.deltaTime;
                Ball[0].transform.position=transform.Find("DriblePos").position;
                if(player)
                {
                    enemyGoal = GameObject.FindGameObjectsWithTag("enemyGoal");
                    agent.SetDestination(enemyGoal[0].transform.position);
                }
                if(!player)
                {
                    playerGoal = GameObject.FindGameObjectsWithTag("playerGoal");
                    agent.SetDestination(playerGoal[0].transform.position);
                }
            }
        }
        if(!attacker)
        {

        }
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag =="Ball"&&HoldBall==false&&attacker)
        {
            
            HoldBall=true;
            anim.SetBool("HoldBall",HoldBall);
            transform.Find("HoldingBall").gameObject.SetActive(true);
            Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
        }
    
    }
}
