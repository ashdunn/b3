using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dMoverScript : MonoBehaviour
{
    public Transform goal;
    public Transform goal1;
    public Transform goal2;
    public Transform goal3;
    private UnityEngine.AI.NavMeshAgent agent;
    private int curGoal;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.destination = goal.position;
        curGoal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 2)
        {
            curGoal++;
            curGoal = curGoal % 4;
            if (curGoal == 0)
                agent.destination = goal.position;
            else if (curGoal == 1)
                agent.destination = goal1.position;
            else if (curGoal == 2)
                agent.destination = goal2.position;
            else if (curGoal == 3)
                agent.destination = goal3.position;
        }
    }
}
