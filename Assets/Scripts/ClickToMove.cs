using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMove : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent agent;
    //private UnityEngine.AI.NavMeshObstacle obstacle;
    private bool selected;
    private bool move;
    private Vector3 MoveTo;
    public Transform goal;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //obstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        selected = false;
        move = true;
        agent.destination = goal.position;

        //obstacle.enabled = false;
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected && move)
        {
            //obstacle.enabled = false;
            agent.enabled = true;
            agent.destination = MoveTo;
            agent.isStopped = false;
        }

    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
    }

    public void Destination(Vector3 d)
    {
        MoveTo = d;
        move = true;
    }
}