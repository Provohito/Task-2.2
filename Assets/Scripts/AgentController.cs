using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class AgentController : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    NavMeshAgent agent;
    Animator agentAnimator;
    [Header("Работа с Hp персонажа")]
    [SerializeField] private Text currentHPText;
    [SerializeField] private Slider sliderHP;
    [SerializeField] private GameObject player;

    [Space(10)]
    [SerializeField] private int maxHP;
    [SerializeField] private int minHP;

    public bool die;

    public int currentHP;

    [SerializeField]
    GameObject uiController;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agentAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        sliderHP.maxValue = maxHP;
        sliderHP.minValue = minHP;

        currentHP = maxHP;
    }

    private void Update()
    {
        
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
            agentAnimator.SetBool("walk", false);
        }

        sliderHP.value = currentHP;
        currentHPText.GetComponent<Text>().text = string.Format("{0:0}", currentHP);

        HPCheck();
        PlayerDeath();
    }
    private void HPCheck()
    {
        if (currentHP >= maxHP)
            currentHP = maxHP;
    }

    private void PlayerDeath()
    {
        if (currentHP <= minHP)
        {
            currentHP = minHP;
            
            if (die == false)
            {
                die = true;
                StopAllCoroutines();
                StartCoroutine(DieAgent());
            }
            
        }
    }

    IEnumerator DieAgent()
    {
        yield return new WaitForSeconds(1f);
        agentAnimator.Play("Dying");
        yield return new WaitForSeconds(4f);
        Destroy(player);
        uiController.GetComponent<UiController>().OpenDiePanel();
    }

}
