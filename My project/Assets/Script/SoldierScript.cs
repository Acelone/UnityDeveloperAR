
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public bool Active, HoldBall, Move, Caught, attacker,player;
    public float radius,timer;
    public GameObject[] attackers;
    public Material currentMat,inactiveMat;
    public ParticleSystem playerPs;
    
    private Animator anim;
    bool collide;

    float NTime;
    public GameObject Ball,Goal,holder,closest;
    AnimatorStateInfo animStateInfo;
    Vector3 startPoint;
    Quaternion startRotation;
    

    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration =100f;
        startPoint= transform.position;
        startRotation=transform.rotation;
        currentMat=transform.Find("Alpha_Surface").GetComponent<SkinnedMeshRenderer>().material;
        timer=0;
        
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
        anim.SetBool("Move", Move);
        anim.SetBool("HoldBall",HoldBall);
        anim.SetBool("Caught", Caught);
        transform.Find("Arrow").gameObject.SetActive(Active);
        animStateInfo = anim.GetCurrentAnimatorStateInfo (0);
        NTime = animStateInfo.normalizedTime;
        Ball = GameObject.FindGameObjectWithTag("Ball");
        for(int i =0;i<attackers.Length;i++)
        {
            if (attackers[i].GetComponent<SoldierScript>().HoldBall)
            {
                holder=attackers[i];
                break;
            }
            
        }
        if(playerPs.isStopped&&collide){
            Destroy(this.gameObject);
        }
        
        if(Active){
            if(attacker)
            {   
                if(player)
                {
                    findPlayer();
                    Goal = GameObject.FindGameObjectWithTag("enemyGoal");
                }
                else
                {
                    findEnemy(); 
                    Goal = GameObject.FindGameObjectWithTag("playerGoal");
                }
                Move=true;
                if(holder==null)
                {
                    gotoBall();
                }
                if(holder==this.gameObject){
                    
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("dribble"))
                    {
                        agent.speed = 25f*0.75f*Time.deltaTime;
                        Ball.transform.position=transform.Find("DriblePos").position;
                        agent.SetDestination(Goal.transform.position);
                    }
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("Pass")&&NTime > 0.1f)
                    {
                        closest=findnearestattacker();
                        transform.rotation= Quaternion.LookRotation(Vector3.RotateTowards
                            (transform.forward, closest.transform.position-transform.position, Time.deltaTime, 0.0f));
                        Ball.GetComponent<BallControl>().pass(closest);
                        Move=false;
                        agent.isStopped =true;
                        timer=2.5f;
                        Inactive();
                    }
                }
                if(holder!=this.gameObject&&holder!=null)
                {
                    
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jog")&&NTime > 0.35f)
                    {
                        agent.speed = 25f*1.5f*Time.deltaTime;
                        transform.rotation= startRotation;
                    }
                }
            }
            if(!attacker)
            {
                if(!player)
                {
                    findPlayer();
                }
                else
                {
                    findEnemy(); 
                }
                if(Caught)
                {
                    if(anim.GetCurrentAnimatorStateInfo(0).IsName("Tackle")&&NTime > 1f)
                    {
                    HoldBall=false;
                    agent.speed = 25f*2f*Time.deltaTime;
                    agent.SetDestination(startPoint);
                    }
                    if(Vector3.Distance(transform.position,startPoint)<=0.5f)
                    {
                        Move=false;
                        agent.isStopped =true;
                        timer=4f;
                        Inactive();
                    }
                }
                else{
                    transform.Find("Arrow/Arrow").gameObject.SetActive(Move);
                    transform.Find("Arrow/CircleYel").gameObject.transform.localScale=new Vector3(radius,radius,1);
                    if(1.5<Vector3.Distance(transform.position,holder.transform.position)&&Vector3.Distance(transform.position,holder.transform.position)<radius)
                    {
                        agent.speed = 25f*1f*Time.deltaTime;
                        Move=true;
                        agent.SetDestination(holder.transform.position);
                    }
                    if(1.5>=Vector3.Distance(transform.position,holder.transform.position))
                    {
                        Caught=true;
                        HoldBall=true;
                        holder.GetComponent<SoldierScript>().Caught=true;
                    }
                }
            }
        }
        if(!Active&&timer>0&&anim.GetCurrentAnimatorStateInfo(0).IsName("kneel")&&NTime > 0.35f)
        {
            timer-=Time.deltaTime; 
        }
        if(timer<=0)
        {
            active();
        }
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag =="Ball"&&HoldBall==false&&attacker)
        {
            HoldBall=true;
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if(((other.gameObject.tag=="playerGoal"&&!player)||(other.gameObject.tag=="enemyGoal"&&player))&&this.HoldBall)
        {
            GameObject.Find("/EventSystem").GetComponent<GameControl>().win=true;
            GameObject.Find("/EventSystem").GetComponent<GameControl>().endTurn=true;
        }
        if((other.gameObject.tag=="playerFence"||other.gameObject.tag=="playerGoal")&&!player||(other.gameObject.tag=="enemyFence"||other.gameObject.tag=="enemyGoal")&&player)
        {
            playerPs.Play();
            collide=true;
        }
        
    }
    void findEnemy()
    {
        attackers= GameObject.FindGameObjectsWithTag("Enemy");
        radius =GameObject.FindGameObjectWithTag("EnemyField").gameObject.transform.localScale.x*0.35f;
    }
    void findPlayer()
    {
        attackers= GameObject.FindGameObjectsWithTag("Player");
        radius =GameObject.FindGameObjectWithTag("PlayerField").gameObject.transform.localScale.x*0.35f;
    }
    void Inactive()
    {
        transform.Find("Alpha_Surface").GetComponent<SkinnedMeshRenderer>().material=inactiveMat;
        Active=false;
        HoldBall=false;
        Caught=false;
        GetComponent<NavMeshAgent>().enabled=false;
    }
    void active()
    {   Active=true;
        GetComponent<NavMeshAgent>().enabled=true;
        agent.isStopped =false;
        transform.Find("Alpha_Surface").GetComponent<SkinnedMeshRenderer>().material=currentMat;
    }
    void gotoBall()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Jog")&&NTime > 0.35f)
                    {
                        agent.speed = 25f*1.5f*Time.deltaTime;
                        agent.SetDestination(Ball.transform.position);
                        
                    }
    }
    GameObject findnearestattacker()
    {
        GameObject nearest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
            foreach (GameObject go in attackers)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance && go!=this.gameObject)
                 {
                    nearest = go;
                    distance = curDistance;
                 }
            }
            if(nearest==null)
                {
                    transform.Find("/EventSystem").gameObject.GetComponent<GameControl>().win=false;
                    transform.Find("/EventSystem").gameObject.GetComponent<GameControl>().endTurn=true;
                }
            return nearest;
    }
    
   
}
