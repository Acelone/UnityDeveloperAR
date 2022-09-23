﻿

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{
    public GameObject Soldier,PlayerEnergyBar,EnemyEnergyBar,NewSoldier;
    public List<GameObject> PlayerSoldiers = new List<GameObject>();
    public List<GameObject> EnemySoldiers = new List<GameObject>();
    public Material enemyMat, playerMat,enemyJointMat, playerJointMat;
    public EnergyBar playerEnergy, enemyEnergy;
    int onlyOne;
    GameControl systems;
    Vector3 p1;

    // Start is called before the first frame update
    void Start()
    {
        playerEnergy=PlayerEnergyBar.GetComponent<EnergyBar>();
        enemyEnergy=EnemyEnergyBar.GetComponent<EnergyBar>();
        systems=GetComponent<GameControl>();
        onlyOne=0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            p1= Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                            
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(p1);
                if(Physics.Raycast(ray,out hit, 50000.0f))
                {
                    if(!systems.penalty)
                    {
                        if(systems.attacker)
                        {
                            if(hit.transform.gameObject.tag=="PlayerField"&&playerEnergy.energy>=2)
                            {
                                playerEnergy.time=playerEnergy.time-4f;
                                NewSoldier= Instantiate(Soldier);
                                deployPlayer(NewSoldier);
                                NewSoldier.transform.position=hit.point;
                            }
                            if(hit.transform.gameObject.tag=="EnemyField"&&enemyEnergy.energy>=3)
                            {
                                enemyEnergy.time=enemyEnergy.time-6f;
                                NewSoldier= Instantiate(Soldier);
                                deployEnemy(NewSoldier);
                                NewSoldier.transform.position=hit.point;
                            }
                        }
                        if(!systems.attacker)
                        {
                            if(hit.transform.gameObject.tag=="PlayerField"&&playerEnergy.energy>=3)
                            {
                                playerEnergy.time=playerEnergy.time-6f;
                                NewSoldier= Instantiate(Soldier);
                                deployPlayer(NewSoldier);
                                NewSoldier.transform.position=hit.point;
                            }
                            if(hit.transform.gameObject.tag=="EnemyField"&&enemyEnergy.energy>=2)
                            {
                                enemyEnergy.time=enemyEnergy.time-4f;
                                NewSoldier= Instantiate(Soldier);
                                deployEnemy(NewSoldier);
                                NewSoldier.transform.position=hit.point;
                            }
                        }
                    }
                    if(systems.penalty&&onlyOne==0)
                    {
                        playerEnergy.time=playerEnergy.time-4f;
                        NewSoldier= Instantiate(Soldier);
                        deployPlayer(NewSoldier);
                        NewSoldier.transform.position=hit.point;
                        systems.penaltyAttacker=NewSoldier;
                        onlyOne=1;
                    }
                    
                }
            }
        }
    }
    void deployPlayer(GameObject soldier)
    {
        soldier.GetComponent<SoldierScript>().attacker=systems.attacker;
        soldier.GetComponent<SoldierScript>().player=true;
        PlayerSoldiers.Add(soldier);
        soldier.transform.Find("Alpha_Joints").GetComponent<SkinnedMeshRenderer>().material=playerJointMat;
        soldier.transform.Find("Alpha_Surface").GetComponent<SkinnedMeshRenderer>().material=playerMat;
        soldier.transform.parent = GameObject.Find("Field/Player Soldier").transform;
        soldier.transform.gameObject.tag="Player";
    }
    void deployEnemy(GameObject soldier)
    {
        soldier.GetComponent<SoldierScript>().attacker=!systems.attacker;
        soldier.GetComponent<SoldierScript>().player=false;
        EnemySoldiers.Add(soldier);
        soldier.transform.Rotate(0f,180f,0f);
        soldier.transform.Find("Alpha_Joints").GetComponent<SkinnedMeshRenderer>().material=enemyJointMat;
        soldier.transform.Find("Alpha_Surface").GetComponent<SkinnedMeshRenderer>().material=enemyMat;
        soldier.transform.parent = GameObject.Find("Field/Enemy Soldier").transform;
        soldier.transform.gameObject.tag="Enemy";
    }
}
