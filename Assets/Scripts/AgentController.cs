using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AgentController : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    NavMeshAgent agent;
    Animator agentAnimator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agentAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Debug.Log(agent.remainingDistance);
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                
                agent.SetDestination(hit.point);
                
            }
            
        }
        if (agent.speed > 0)
        {
            agentAnimator.SetBool("walk", true);
        }
        if (agent.remainingDistance <= 1)
        {
            Debug.Log("stop");
            agentAnimator.SetBool("walk", false);
        }
    }
}
