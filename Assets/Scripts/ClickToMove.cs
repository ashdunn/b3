using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Util;

public class ClickToMove : MonoBehaviour
{

    //private UnityEngine.AI.NavMeshObstacle obstacle;
    public Transform goal;

    private UnityEngine.AI.NavMeshAgent agent;
    private bool selected;
    private bool move;
    // private Vector3 MoveTo;
    private Vector3 hDest;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //obstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>();
        selected = false;
        move = true;
        Destination(goal.position);

        //obstacle.enabled = false;
        agent.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if (selected && move)
        // {
        //     //obstacle.enabled = false;
        //     agent.enabled = true;
        //     // agent.destination = MoveTo;
        //     agent.isStopped = false;
        // }

        Vector3 hTrans = transform.position - vert(transform.position.y);
        print(hTrans + ", " + hDest);
        if ((hDest - hTrans).sqrMagnitude <= pow2(agent.stoppingDistance))
        {
            move = false;
        }

        if (move)
        {
            // agent.enabled = true;
            agent.isStopped = false;
        }
        else
        {
            // agent.enabled = false;
            agent.isStopped = true;
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
        agent.destination = d;
        hDest = d - vert(d.y);
        move = true;
    }

    public bool canMove()
    {
        return move;
    }

    private bool canMove(GameObject go)
    {
        return go.GetComponent<ClickToMove>().canMove();
    }

    public Vector3 getDestination()
    {
        return agent.destination;
    }

    private Vector3 getDestination(GameObject go)
    {
        return go.GetComponent<ClickToMove>().getDestination();
    }

    void OnTriggerEnter(Collider coll)
    {
        GameObject other = coll.gameObject;
        // print(coll + ", " + other);
        // print(other.GetComponent<ClickToMove>());
        if (canMove() && other.CompareTag("Agent"))
            print(getDestination(other) + ", " + getDestination() + ", " + !canMove(other));
        if (canMove() && other.CompareTag("Agent") && getDestination(other) == getDestination() && !canMove(other))
        {
            move = false;
        }
    }
}
