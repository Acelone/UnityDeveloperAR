using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    GameObject passTo;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, passTo.transform.position, 1.5f*Time.deltaTime);
    }

    public void pass(GameObject go)
    {
        passTo=go;
        return;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="playerFence"||other.gameObject.tag=="enemyFence"||
            other.gameObject.tag=="playerGoal"||other.gameObject.tag=="enemyGoal"||other.gameObject.tag=="Maze")
        {
            transform.position= Random.insideUnitSphere 
                * 5f+ transform.Find("Field").position;
        }
        
    }
}
