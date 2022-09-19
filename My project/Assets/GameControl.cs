using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public GameObject EnemyBar,PlayerBar,Ball,PlayerField,EnemyField;
    public Text textTimer,enemyTitle,playerTitle;
    public float timer;
    public int turn;
    public bool attacker;
    

    // Start is called before the first frame update
    void Start()
    {
        timer=140f;
        turn=1;
        GameObject NewBall= Instantiate (Ball,Random.insideUnitSphere * 5f+ PlayerField.transform.position,  Random.rotation);
        NewBall.transform.position= new Vector3(NewBall.transform.position.x,0.65f,NewBall.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        timer-=Time.deltaTime; 
        textTimer.text=timer.ToString("F0");
        if(turn%2==0)
        {
            attacker=false;
            enemyTitle.text= "Enemy (Attacker)";
            playerTitle.text= "Player (Defender)";
            
        }
        if(turn%2==1)
        {
            attacker=true;
            enemyTitle.text= "Enemy (Defender)";
            playerTitle.text= "Player (Attacker)";
        }
    }
}
