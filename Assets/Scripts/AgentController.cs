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
    [SerializeField]
    GameObject diePanel;

    float distance;
    GameObject bot;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

        sliderHP.value = currentHP;
        currentHPText.GetComponent<Text>().text = string.Format("{0:0}", currentHP);

        HPCheck();
        PlayerDeath();

        if (bot == null)
        {
            return;
        }
        else
        {
            if (distance < 4f) //если дистанция меньше указанного значения
            {
                if (player.GetComponent<AgentController>().currentHP != 0)
                {
                    SetRotarion();
                }
            }
            else if (distance < 8f) //если дистанция до игрока меньше радиуса видимости
            {
                SetRotarion();

            }
        }

        

        
    }
    
    void SetRotarion()
    {
        Vector3 targetPoint = bot.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
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
                StartCoroutine(DieAgent());
            }
            
        }
    }

    IEnumerator DieAgent()
    {
        yield return new WaitForSeconds(1f);
        Destroy(player);
        uiController.GetComponent<UiController>().OpenDiePanel(diePanel);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "bot")
        {
            distance = Vector3.Distance(transform.position, other.transform.position);
            bot = other.gameObject;
        }
    }


}
