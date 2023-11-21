using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestAgent : MonoBehaviour
{
    public GameObject destinationObject;
    public NavMeshAgent navMeshAgent;
    void Start()
    {
        destinationObject = GameObject.Find("Destination");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate() {
        navMeshAgent.SetDestination(destinationObject.transform.position);
    }
}
