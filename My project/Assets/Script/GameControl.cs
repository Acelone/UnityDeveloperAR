using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameControl : MonoBehaviour
{
    public GameObject EnemyBar,PlayerBar,Ball,PlayerField,EnemyField,NewBall,penaltyAttacker;
    public Text textTimer,enemyTitle,playerTitle;
    public float timer,sceneWidth =10;
    public int turn,lastTurn,playerWin,enemyWin;
    public bool attacker,endTurn,win,penalty,ARMode;
    public bool pause = false;
    public GameObject PauseCanvas, confirmCanvas,endTurnCanvas,curCanvas;
    PlayerControl playerCon;
    EnergyBar playerBar, enemyBar;
    Camera _camera;
    float desiredHalfHeight;
    

    // Start is called before the first frame update
    void Start()
    {
         _camera = Camera.main.GetComponent<Camera>();
        playerCon=GetComponent<PlayerControl>();
        playerBar= playerCon.playerEnergy.GetComponent<EnergyBar>();
        enemyBar= playerCon.enemyEnergy.GetComponent<EnergyBar>();
        timer=140f;
        lastTurn=0;
        turn=1;
        endTurn =false;
        PauseCanvas.SetActive(false);
        confirmCanvas.SetActive(false);
        endTurnCanvas.SetActive(false);
        playerWin=0;
        enemyWin=0;
        penalty=false;
        ARMode=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            float unitsPerPixel = sceneWidth / Screen.width;

            desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
             _camera.orthographicSize = desiredHalfHeight;
        }
        
        timer-=Time.deltaTime; 
        textTimer.text=timer.ToString("F0");
        
        if(endTurn&&curCanvas!=endTurnCanvas)
            {
                endTurnCanvas.SetActive(true);
                curCanvas=endTurnCanvas;
                Time.timeScale=0f;
            }
            if(!endTurn)
            {
                endTurnCanvas.SetActive(false);
                Time.timeScale=1f;
                
            }
        if(!penalty)
        {
            if(turn%2==0)
            {
                attacker=false;
                enemyTitle.text= "Enemy (Attacker) "+enemyWin;
                playerTitle.text= "Player (Defender) "+playerWin;
                if(lastTurn!=turn)
                {
                    NewBall= Instantiate (Ball,Random.insideUnitSphere 
                        * 5f+ EnemyField.transform.position,  Random.rotation);
                    lastTurn=turn;
                }
            }
            if(turn%2==1)
            {
                attacker=true;
                enemyTitle.text= "Enemy (Defender) "+enemyWin;
                playerTitle.text= "Player (Attacker) "+playerWin;
                if(lastTurn!=turn)
                {
                    NewBall= Instantiate (Ball,Random.insideUnitSphere 
                        * 5f+ PlayerField.transform.position,  Random.rotation);
                    lastTurn=turn;
                }
            }
            if(timer>0&& endTurnCanvas.activeSelf==true&&timer<139f)
            {
                if(attacker&&win||!attacker&&!win)
                {
                    endTurnCanvas.transform.Find("Text (1)").gameObject.
                        GetComponent<Text>().text = "Player Win";
                    playerWin+=1;
                    
                }
                if(!attacker&&win||attacker&&!win)
                {
                    endTurnCanvas.transform.Find("Text (1)").gameObject.
                        GetComponent<Text>().text = "Enemy Win";
                    enemyWin+=1;
                }
            }
            if(endTurnCanvas.activeSelf==true)
            {
                if(turn>=5)
                {
                    endTurnCanvas.transform.Find("NextTurn").gameObject.SetActive(false);
                    if(enemyWin>playerWin)
                    {
                        endTurnCanvas.transform.Find("Text").gameObject.
                            GetComponent<Text>().text = "GameOver";
                        endTurnCanvas.transform.Find("Text (1)").gameObject.
                            GetComponent<Text>().text = "Player Lose";
                    }
                    if(enemyWin<playerWin)
                    {
                        endTurnCanvas.transform.Find("Text (1)").gameObject.
                            GetComponent<Text>().text = "Player Win";
                    }
                    if(enemyWin==playerWin)
                    {
                        endTurnCanvas.transform.Find("Exit").gameObject.SetActive(false);
                        endTurnCanvas.transform.Find("PenaltyGame").gameObject.SetActive(true);
                        endTurnCanvas.transform.Find("Text").gameObject.
                            GetComponent<Text>().text = "DRAW";
                        endTurnCanvas.transform.Find("Text (1)").gameObject.
                            GetComponent<Text>().text = "Proceed To Penalty Game";
                    }
                }
                timer=140f;
            }
            NewBall.transform.position= new Vector3(NewBall.transform.position.x,
                0.5f,NewBall.transform.position.z);
            if(timer<=0)
            {
                endTurn=true;
                endTurnCanvas.transform.Find("Text (1)").gameObject.
                    GetComponent<Text>().text = "Draw";
                playerWin+=1;
                enemyWin+=1;
                timer=140f;
            }
        }
        if(penalty)
        {
            endTurnCanvas.transform.Find("NextTurn").gameObject.SetActive(false);
            endTurnCanvas.transform.Find("PenaltyGame").gameObject.SetActive(false);
            endTurnCanvas.transform.Find("Exit").gameObject.SetActive(true);
            NewBall.transform.position= new Vector3(NewBall.transform.position.x,
                0.65f,NewBall.transform.position.z);
           if(timer<=0)
            {
                endTurn=true;
                endTurnCanvas.transform.Find("Text").gameObject.
                    GetComponent<Text>().text = "GameOver";
                endTurnCanvas.transform.Find("Text (1)").gameObject.
                    GetComponent<Text>().text = "Player Lose";
            } 
            if(timer>0&& penaltyAttacker.GetComponent<SoldierScript>().HoldBall)
            {
                endTurn=true;
                endTurnCanvas.transform.Find("Text").gameObject.
                    GetComponent<Text>().text = "Congratulation";
                endTurnCanvas.transform.Find("Text (1)").gameObject.
                    GetComponent<Text>().text = "Player Win";
            }
            
        }
        
        if (Input.GetKey(KeyCode.Escape))
        {   
            if(pause==true)
            {
                resume();
            }
            else
            {
                paused();
            }
        }
    }
    public void resume()
    {
        PauseCanvas.SetActive(false);
        Time.timeScale=1f;
        pause= false;
    }
    void paused()
    {
        PauseCanvas.SetActive(true);
        curCanvas=PauseCanvas;
        Time.timeScale=0f;
        pause= true;
    }
    public void confirmation()
    {
        curCanvas.SetActive(false);
        confirmCanvas.SetActive(true);
    }
    public void returnPause()
    {
        confirmCanvas.SetActive(false);
        curCanvas.SetActive(true);
    }
    public void back()
    {
        SceneManager.LoadScene(0);  
    }
    public void nextTurn()
    {
        Destroy(NewBall);
        turn+=1;
        timer=140f;
        playerBar.time=0;
        enemyBar.time=0;
        foreach(GameObject go in playerCon.PlayerSoldiers)
        {
            Destroy(go);
        }
        foreach(GameObject go in playerCon.EnemySoldiers)
        {
            Destroy(go);
        }
        endTurn=false;
        curCanvas=null;
    }
    public void penaltyTurn()
    {
        Destroy(NewBall);
        timer=140f;
        turn+=1;
        playerBar.time=0;
        enemyBar.time=0;
        penalty=true;
        foreach(GameObject go in playerCon.PlayerSoldiers)
        {
            Destroy(go);
        }
        foreach(GameObject go in playerCon.EnemySoldiers)
        {
            Destroy(go);
        }
        endTurn=false;
        curCanvas=null;
        GetComponent<MazeGenerator>().generate=true;
        if(lastTurn!=turn)
            {
                NewBall= Instantiate (Ball,Random.insideUnitSphere 
                    * 5f+ PlayerField.transform.position+EnemyField.transform.position,  Random.rotation);
                lastTurn=turn;
            }
    }
    public void ARButton()
    {
        if(!ARMode)
        {
            ARMode=true;
        }
        else
        {
            ARMode=false;
        }
        GameObject.Find("AR Session Origin").GetComponent<ARMode>().AROn=ARMode;
        GameObject.Find("AR Session Origin").GetComponent<ARMode>().clicked=ARMode;
    }
}
