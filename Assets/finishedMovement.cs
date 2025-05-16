using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class finishedMovement : MonoBehaviour
{
    public float standRange;
    public NavMeshAgent agent;
    bool finished;
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, agent.destination) <= standRange)
        {
            finished = true;
        }
        else { finished = false; }
    }

    public bool getFinished() {  return finished; }
}
