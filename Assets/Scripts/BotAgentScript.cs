using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BotAgentScript : MonoBehaviour
{
    //  Напишите позицию, куда должен идти бот(Брал только по х)
    [SerializeField]
    int pointToMove;
	private NavMeshAgent botAgent;
	[SerializeField]
	GameObject player;

	float rotationSpeed = 4;

	Vector3 startPointBot;
	Vector3 endPointBot;
	int timeWait = 3;
	float curTimeout = 0;

	bool isTarget;

	Animator botAnimator;

	float visible = 8f;// Видимость бота
	[Header("Работа с Hp персонажа")]
	[SerializeField] private Text currentHPText;
	[SerializeField] private Slider sliderHP;


	[Space(10)]
	[SerializeField] private int maxHP;
	[SerializeField] private int minHP;

	bool die;

	public int currentHP;

	[SerializeField]
	GameObject uiController;
	[SerializeField]
	GameObject diePanel;


	private void Awake()
    {
		botAgent = GetComponent<NavMeshAgent>();
		botAnimator = GetComponent<Animator>();
	}
    void Start()
	{
		startPointBot = transform.position;
		endPointBot = new Vector3(pointToMove, transform.position.y, transform.position.z);

		sliderHP.maxValue = maxHP;
		sliderHP.minValue = minHP;

		currentHP = maxHP;

	}

	void Update()
	{
		HPCheck();
		BotDeath();

		curTimeout += Time.deltaTime;
		if (!botAgent.hasPath & curTimeout > timeWait)
		{
			isTarget = !isTarget;
			curTimeout = 0;
		}
		

		float distance = Vector3.Distance(transform.position, player.transform.position);
		if (distance < 2f) //если дистанция меньше указанного значения
		{
            if (player.GetComponent<AgentController>().currentHP != 0)
			{ 
				StartCoroutine(DamageTake());
				botAnimator.SetBool("isFight", true);
				player.GetComponent<Animator>().Play("Boxing");
			}
		}
		else if (distance < visible) //если дистанция до игрока меньше радиуса видимости
		{
			
			botAgent.destination = player.transform.position; //и передаем агенту навигации координаты игрока, чтобы идти к нему
			SetRotation(player.transform.position);
		}
		else
		{
			if (isTarget)
			{
				Move(endPointBot);
			}
			else
			{
				Move(startPointBot);
			}
		}

		sliderHP.value = currentHP;
		currentHPText.GetComponent<Text>().text = string.Format("{0:0}", currentHP);


		
	}

	void Move(Vector3 moveVector)
    {
		botAgent.SetDestination(moveVector);
	}

	void SetRotation(Vector3 lookAt)
	{
		Vector3 targetPoint = player.transform.position;
		Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
	}

	IEnumerator DamageTake()
    {
		yield return new WaitForSeconds(1f);
		player.GetComponent<AgentController>().currentHP -= Random.Range(1,15);
		currentHP -= Random.Range(20, 50);
		StopAllCoroutines();
    }
	private void HPCheck()
	{
		if (currentHP >= maxHP)
			currentHP = maxHP;
	}

	private void BotDeath()
	{
		if (currentHP <= minHP)
		{
			currentHP = minHP;
			die = true;
			StopAllCoroutines();
			StartCoroutine(DieBot());
			

		}
	}
	
	IEnumerator DieBot()
	{

		botAnimator.Play("Flying Back Death");
		player.GetComponent<Animator>().Play("Idle");
		Debug.Log("Die");
		Destroy(this.gameObject);
		//uiController.GetComponent<UiController>().OpenDiePanel(diePanel);
		yield return new WaitForSeconds(0.5f);
	}


}
